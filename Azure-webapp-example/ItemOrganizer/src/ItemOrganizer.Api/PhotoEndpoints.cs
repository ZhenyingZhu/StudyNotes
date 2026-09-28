using System.Security.Cryptography;
using Azure;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace ItemOrganizer.Api;

public static class PhotoEndpoints
{
    private const long MaximumPhotoBytes = 10 * 1024 * 1024;
    private const string UploadOperation = "photo-upload";
    private static readonly SemaphoreSlim ImageValidationSlots = new(2, 2);

    public static void MapPhotoEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1");
        api.MapPost("/photos", UploadAsync)
            .RequireAuthorization(AuthorizationPolicies.Write)
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<PhotoResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status413PayloadTooLarge)
            .ProducesProblem(StatusCodes.Status415UnsupportedMediaType)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable);
        api.MapDelete("/photos/{photoId:guid}", DeleteAsync)
            .RequireAuthorization(AuthorizationPolicies.Write);
    }

    public static async Task<IResult> GetContentAsync(
        Guid photoId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IPhotoStorage storage,
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var photo = await dbContext.Photos.AsNoTracking().SingleOrDefaultAsync(
            entity =>
                entity.Id == photoId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null,
            cancellationToken);
        if (photo is null)
        {
            return NotFound(context);
        }

        try
        {
            var configuredMinutes = configuration.GetValue(
                "PhotoStorage:ReadUrlMinutes",
                5);
            var lifetime = TimeSpan.FromMinutes(
                Math.Clamp(configuredMinutes, 1, 15));
            var uri = await storage.CreateReadUriAsync(
                photo.BlobName,
                lifetime,
                cancellationToken);
            context.Response.Headers.CacheControl = "private, no-store";
            return Results.Redirect(uri.ToString());
        }
        catch (FileNotFoundException exception)
        {
            loggerFactory.CreateLogger("Photos.Content").LogWarning(
                exception,
                "Photo blob was not found. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
            return DependencyUnavailable(context);
        }
        catch (Exception exception) when (
            exception is RequestFailedException or InvalidOperationException)
        {
            loggerFactory.CreateLogger("Photos.Content").LogError(
                exception,
                "Photo read access failed. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
            return DependencyUnavailable(context);
        }
    }

    private static async Task<IResult> UploadAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IPhotoStorage storage,
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        if (!context.Request.HasFormContentType)
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "A multipart/form-data request with one file part is required.");
        }

        IFormCollection form;
        try
        {
            form = await context.Request.ReadFormAsync(cancellationToken);
        }
        catch (InvalidDataException)
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "The multipart request is malformed.");
        }

        if (form.Files.Count != 1
            || !string.Equals(
                form.Files[0].Name,
                "file",
                StringComparison.Ordinal))
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "Exactly one file part named 'file' is required.");
        }

        var file = form.Files[0];
        if (file.Length > MaximumPhotoBytes)
        {
            return Problem(
                context,
                StatusCodes.Status413PayloadTooLarge,
                "Photo size cannot exceed 10 MiB.");
        }
        if (file.Length <= 0)
        {
            return Unprocessable(context, "The photo file is empty.");
        }

        ValidatedPhoto validated;
        try
        {
            validated = await ValidateAsync(file, cancellationToken);
        }
        catch (UnsupportedPhotoException exception)
        {
            return Problem(
                context,
                StatusCodes.Status415UnsupportedMediaType,
                exception.Message);
        }
        catch (InvalidPhotoException exception)
        {
            return Unprocessable(context, exception.Message);
        }

        await using var content = validated.Content;
        var user = currentUserAccessor.GetRequired();
        var idempotencyKey = context.Request.Headers["Idempotency-Key"]
            .FirstOrDefault()?.Trim();
        if (idempotencyKey?.Length > 200)
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "Idempotency-Key cannot exceed 200 characters.");
        }

        var requestHash = $"{validated.Sha256}:{validated.ContentType}";
        if (!string.IsNullOrEmpty(idempotencyKey))
        {
            var replay = await FindReplayAsync(
                dbContext,
                user,
                idempotencyKey,
                requestHash,
                context,
                cancellationToken);
            if (replay is not null)
            {
                return replay;
            }
        }

        var maximumPhotos = Math.Max(
            1,
            configuration.GetValue("PhotoStorage:MaximumPhotosPerOwner", 1000));
        var activePhotoCount = await dbContext.Photos.CountAsync(
            entity =>
                entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null,
            cancellationToken);
        if (activePhotoCount >= maximumPhotos)
        {
            context.Response.Headers.RetryAfter = "3600";
            return Problem(
                context,
                StatusCodes.Status429TooManyRequests,
                "The active photo quota has been reached.");
        }

        var now = DateTimeOffset.UtcNow;
        var photoId = Guid.NewGuid();
        var blobName = $"photos/{Guid.NewGuid():N}";
        var photo = new Photo(
            photoId,
            user.TenantId,
            user.OwnerObjectId,
            blobName,
            validated.ContentType,
            validated.Content.Length,
            validated.Width,
            validated.Height,
            validated.Sha256,
            now.AddDays(30),
            now);
        IdempotencyRecord? idempotency = null;
        if (!string.IsNullOrEmpty(idempotencyKey))
        {
            idempotency = new IdempotencyRecord(
                Guid.NewGuid(),
                user.TenantId,
                user.OwnerObjectId,
                UploadOperation,
                idempotencyKey,
                requestHash,
                now.AddHours(24),
                now);
            idempotency.Complete("photo", photo.Id, now);
        }

        try
        {
            content.Position = 0;
            await storage.UploadAsync(
                blobName,
                content,
                validated.ContentType,
                cancellationToken);

            dbContext.Photos.Add(photo);
            if (idempotency is not null)
            {
                dbContext.IdempotencyRecords.Add(idempotency);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            await TryDeleteBlobAsync(
                storage,
                blobName,
                loggerFactory,
                context.TraceIdentifier,
                cancellationToken);
            loggerFactory.CreateLogger("Photos.Upload").LogError(
                exception,
                "Photo metadata persistence failed. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
            if (!string.IsNullOrEmpty(idempotencyKey))
            {
                dbContext.ChangeTracker.Clear();
                var replay = await FindReplayAsync(
                    dbContext,
                    user,
                    idempotencyKey,
                    requestHash,
                    context,
                    cancellationToken);
                if (replay is not null)
                {
                    return replay;
                }
            }
            return DependencyUnavailable(context);
        }
        catch (RequestFailedException exception)
        {
            loggerFactory.CreateLogger("Photos.Upload").LogError(
                exception,
                "Photo blob upload failed. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
            return DependencyUnavailable(context);
        }

        SetPhotoHeaders(context, photo);
        return Results.Created(
            $"/api/v1/photos/{photo.Id}",
            ToPhotoResponse(photo));
    }

    private static async Task<IResult?> FindReplayAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUser user,
        string key,
        string requestHash,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.IdempotencyRecords
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity =>
                    entity.TenantId == user.TenantId
                    && entity.OwnerObjectId == user.OwnerObjectId
                    && entity.Operation == UploadOperation
                    && entity.Key == key,
                cancellationToken);
        if (record is null)
        {
            return null;
        }
        if (!string.Equals(
            record.RequestHash,
            requestHash,
            StringComparison.Ordinal))
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "Idempotency-Key was already used for a different request.");
        }
        if (record.Status != IdempotencyStatus.Completed
            || record.ResourceId is null)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "The matching request is still being processed.");
        }

        var photo = await dbContext.Photos.AsNoTracking().SingleOrDefaultAsync(
            entity =>
                entity.Id == record.ResourceId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null,
            cancellationToken);
        if (photo is null)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "The result of the matching request is no longer available.");
        }

        SetPhotoHeaders(context, photo);
        context.Response.Headers.Location = $"/api/v1/photos/{photo.Id}";
        return Results.Ok(ToPhotoResponse(photo));
    }

    private static async Task<IResult> DeleteAsync(
        Guid photoId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IPhotoStorage storage,
        ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var photo = await dbContext.Photos.SingleOrDefaultAsync(
            entity =>
                entity.Id == photoId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId,
            cancellationToken);
        if (photo is null)
        {
            return NotFound(context);
        }
        if (photo.RetentionState == PhotoRetentionState.Deleted)
        {
            return Results.NoContent();
        }

        var hasActiveAnalysis = await dbContext.Analyses.AnyAsync(
            entity =>
                entity.PhotoId == photo.Id
                && (entity.Status == AnalysisStatus.Queued
                    || entity.Status == AnalysisStatus.Running),
            cancellationToken);
        var hasItems = await dbContext.Items.AnyAsync(
            entity => entity.PhotoId == photo.Id && entity.DeletedAt == null,
            cancellationToken);
        if (hasItems)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "A photo referenced by inventory items cannot be deleted.");
        }

        try
        {
            photo.RequestDeletion(hasActiveAnalysis, DateTimeOffset.UtcNow);
        }
        catch (DomainException exception)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                exception.Message);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        try
        {
            await storage.DeleteIfExistsAsync(
                photo.BlobName,
                cancellationToken);
            photo.MarkDeleted(DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        }
        catch (RequestFailedException exception)
        {
            loggerFactory.CreateLogger("Photos.Delete").LogError(
                exception,
                "Photo blob deletion failed and remains pending. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
            return DependencyUnavailable(context);
        }
    }

    private static async Task<ValidatedPhoto> ValidateAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var declaredType = file.ContentType.Trim().ToLowerInvariant();
        if (declaredType is not ("image/jpeg" or "image/png" or "image/webp"))
        {
            throw new UnsupportedPhotoException(
                "Only JPEG, PNG, and WebP photos are supported.");
        }

        var content = new MemoryStream((int)file.Length);
        await using (var input = file.OpenReadStream())
        {
            await input.CopyToAsync(content, cancellationToken);
        }
        if (content.Length > MaximumPhotoBytes)
        {
            await content.DisposeAsync();
            throw new InvalidPhotoException(
                "Photo size cannot exceed 10 MiB.");
        }

        try
        {
            var width = 0;
            var height = 0;
            await ImageValidationSlots.WaitAsync(cancellationToken);
            try
            {
                content.Position = 0;
                using var managedStream = new SKManagedStream(
                    content,
                    disposeManagedStream: false);
                using var codec = SKCodec.Create(managedStream);
                if (codec is null)
                {
                    throw new InvalidPhotoException(
                        "The uploaded image is malformed.");
                }

                var detectedType = codec.EncodedFormat switch
                {
                    SKEncodedImageFormat.Jpeg => "image/jpeg",
                    SKEncodedImageFormat.Png => "image/png",
                    SKEncodedImageFormat.Webp => "image/webp",
                    _ => throw new UnsupportedPhotoException(
                        "The uploaded file is not a supported image.")
                };
                if (!string.Equals(
                    detectedType,
                    declaredType,
                    StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnsupportedPhotoException(
                        "The file content does not match its declared media type.");
                }
                if (codec.Info.Width is < 512 or > 8000
                    || codec.Info.Height is < 512 or > 8000)
                {
                    throw new InvalidPhotoException(
                        "Photo dimensions must be between 512 x 512 and 8000 x 8000 pixels.");
                }
                if (codec.FrameCount > 1)
                {
                    throw new InvalidPhotoException(
                        "Animated and multi-frame photos are not supported.");
                }
                width = codec.Info.Width;
                height = codec.Info.Height;

                using var bitmap = new SKBitmap(codec.Info);
                if (codec.GetPixels(
                    codec.Info,
                    bitmap.GetPixels()) != SKCodecResult.Success)
                {
                    throw new InvalidPhotoException(
                        "The uploaded image is malformed.");
                }
            }
            finally
            {
                ImageValidationSlots.Release();
            }

            content.Position = 0;
            var sha256 = Convert.ToHexString(
                await SHA256.HashDataAsync(content, cancellationToken))
                .ToLowerInvariant();
            content.Position = 0;
            return new(
                content,
                declaredType,
                width,
                height,
                sha256);
        }
        catch
        {
            await content.DisposeAsync();
            throw;
        }
    }

    private static async Task TryDeleteBlobAsync(
        IPhotoStorage storage,
        string blobName,
        ILoggerFactory loggerFactory,
        string correlationId,
        CancellationToken cancellationToken)
    {
        try
        {
            await storage.DeleteIfExistsAsync(blobName, cancellationToken);
        }
        catch (RequestFailedException exception)
        {
            loggerFactory.CreateLogger("Photos.Upload").LogError(
                exception,
                "Failed to clean up an uncommitted photo blob. Correlation ID: {CorrelationId}",
                correlationId);
        }
    }

    private static PhotoResponse ToPhotoResponse(Photo photo)
    {
        return new(
            photo.Id,
            photo.ContentType,
            photo.ContentLength,
            photo.Width,
            photo.Height,
            photo.RetentionState,
            null,
            photo.RetainUntil,
            photo.CreatedAt,
            photo.UpdatedAt);
    }

    private static void SetPhotoHeaders(HttpContext context, Photo photo)
    {
        context.Response.Headers.ETag =
            $"\"{photo.ConcurrencyToken:D}\"";
        context.Response.Headers.Location =
            $"/api/v1/photos/{photo.Id}";
    }

    private static IResult NotFound(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status404NotFound,
            "The requested resource does not exist or is not visible to the caller.");
    }

    private static IResult Unprocessable(
        HttpContext context,
        string detail)
    {
        return Problem(
            context,
            StatusCodes.Status422UnprocessableEntity,
            detail);
    }

    private static IResult DependencyUnavailable(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status503ServiceUnavailable,
            "Photo storage is temporarily unavailable.");
    }

    private static IResult Problem(
        HttpContext context,
        int statusCode,
        string detail)
    {
        return Results.Problem(
            statusCode: statusCode,
            detail: detail,
            type: $"https://httpstatuses.com/{statusCode}",
            extensions: new Dictionary<string, object?>
            {
                ["correlationId"] = context.TraceIdentifier
            });
    }

    private sealed record ValidatedPhoto(
        MemoryStream Content,
        string ContentType,
        int Width,
        int Height,
        string Sha256);

    private sealed class UnsupportedPhotoException(string message)
        : Exception(message);

    private sealed class InvalidPhotoException(string message)
        : Exception(message);
}

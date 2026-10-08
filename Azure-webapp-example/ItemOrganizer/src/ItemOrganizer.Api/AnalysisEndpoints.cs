using System.Security.Cryptography;
using System.Text;
using Azure;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Api;

public static class AnalysisEndpoints
{
    private const string CreateOperation = "analysis-create";

    public static void MapAnalysisEndpoints(this WebApplication app)
    {
        var analyses = app.MapGroup("/api/v1")
            .RequireAuthorization(AuthorizationPolicies.Analyze);

        analyses.MapPost(
                "/photos/{photoId:guid}/analyses",
                CreateAsync)
            .Produces<AnalysisResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
        analyses.MapPost(
                "/analyses/{analysisId:guid}/cancel",
                CancelAsync)
            .Produces<AnalysisResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status412PreconditionFailed)
            .ProducesProblem(StatusCodes.Status428PreconditionRequired);
        analyses.MapPost(
                "/analyses/{analysisId:guid}/confirm",
                ConfirmAsync)
            .RequireAuthorization(AuthorizationPolicies.Write)
            .Produces<AnalysisResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
        analyses.MapPost(
                "/containers/{containerId:guid}/photo-analyses",
                CreateForContainerAsync)
            .RequireAuthorization(AuthorizationPolicies.Write)
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<AnalysisResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status413PayloadTooLarge)
            .ProducesProblem(StatusCodes.Status415UnsupportedMediaType)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable);
    }

    private static async Task<IResult> CreateAsync(
        Guid photoId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IConfiguration configuration,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        return await CreateAnalysisAsync(
            photoId,
            null,
            dbContext,
            currentUserAccessor,
            configuration,
            context,
            cancellationToken);
    }

    private static async Task<IResult> CreateForContainerAsync(
        Guid containerId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IPhotoStorage storage,
        IConfiguration configuration,
        ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var containerExists = await dbContext.Containers.AnyAsync(
            container =>
                container.Id == containerId
                && container.TenantId == user.TenantId
                && container.OwnerObjectId == user.OwnerObjectId
                && container.DeletedAt == null,
            cancellationToken);
        if (!containerExists)
        {
            return NotFound(context);
        }

        var upload = await PhotoEndpoints.UploadForWorkflowAsync(
            dbContext,
            currentUserAccessor,
            storage,
            configuration,
            loggerFactory,
            context,
            cancellationToken);
        if (upload.Error is not null)
        {
            return upload.Error;
        }

        return await CreateAnalysisAsync(
            upload.Photo!.Id,
            containerId,
            dbContext,
            currentUserAccessor,
            configuration,
            context,
            cancellationToken);
    }

    private static async Task<IResult> CreateAnalysisAsync(
        Guid photoId,
        Guid? confirmedContainerId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        IConfiguration configuration,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var photoExists = await dbContext.Photos.AnyAsync(
            photo =>
                photo.Id == photoId
                && photo.TenantId == user.TenantId
                && photo.OwnerObjectId == user.OwnerObjectId
                && photo.DeletedAt == null
                && photo.RetentionState == PhotoRetentionState.Active,
            cancellationToken);
        if (!photoExists)
        {
            return NotFound(context);
        }

        var idempotencyKey = context.Request.Headers["Idempotency-Key"]
            .FirstOrDefault()?.Trim();
        if (idempotencyKey?.Length > 200)
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "Idempotency-Key cannot exceed 200 characters.");
        }

        var promptVersion = GetConfiguredValue(
            configuration,
            "Analysis:PromptVersion",
            "2026-09-15.m1");
        var schemaVersion = GetConfiguredValue(
            configuration,
            "Analysis:SchemaVersion",
            "item-organizer.analysis-result.v1");
        var model = GetConfiguredValue(
            configuration,
            "Analysis:Model",
            "deterministic-mock");
        var applicationVersion = typeof(Program).Assembly
            .GetName().Version?.ToString() ?? "development";
        var requestHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(
                    $"{photoId:D}|{confirmedContainerId:D}|{promptVersion}|{schemaVersion}|{model}")))
            .ToLowerInvariant();

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

        var now = DateTimeOffset.UtcNow;
        var analysis = new Analysis(
            Guid.NewGuid(),
            user.TenantId,
            user.OwnerObjectId,
            photoId,
            promptVersion,
            schemaVersion,
            model,
            applicationVersion,
            confirmedContainerId,
            now);
        var outboxMessage = new OutboxMessage(
            Guid.NewGuid(),
            user.TenantId,
            user.OwnerObjectId,
            analysis.Id,
            now);

        dbContext.Analyses.Add(analysis);
        dbContext.OutboxMessages.Add(outboxMessage);
        if (!string.IsNullOrEmpty(idempotencyKey))
        {
            var record = new IdempotencyRecord(
                Guid.NewGuid(),
                user.TenantId,
                user.OwnerObjectId,
                CreateOperation,
                idempotencyKey,
                requestHash,
                now.AddHours(24),
                now);
            record.Complete("analysis", analysis.Id, now);
            dbContext.IdempotencyRecords.Add(record);
        }

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException) when (!string.IsNullOrEmpty(idempotencyKey))
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

            throw;
        }

        return Accepted(context, analysis);
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
                    && entity.Operation == CreateOperation
                    && entity.Key == key,
                cancellationToken);
        if (record is null)
        {
            return null;
        }
        if (!string.Equals(record.RequestHash, requestHash, StringComparison.Ordinal))
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

        var analysis = await dbContext.Analyses.AsNoTracking()
            .SingleOrDefaultAsync(
                entity =>
                    entity.Id == record.ResourceId
                    && entity.TenantId == user.TenantId
                    && entity.OwnerObjectId == user.OwnerObjectId,
                cancellationToken);
        return analysis is null
            ? Problem(
                context,
                StatusCodes.Status409Conflict,
                "The result of the matching request is no longer available.")
            : Accepted(context, analysis);
    }

    private static async Task<IResult> CancelAsync(
        Guid analysisId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            entity =>
                entity.Id == analysisId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId,
            cancellationToken);
        if (analysis is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, analysis.ConcurrencyToken, out var error))
        {
            return error!;
        }

        try
        {
            analysis.RequestCancellation(DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DomainException exception)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                exception.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Problem(
                context,
                StatusCodes.Status412PreconditionFailed,
                "The resource has changed. Refresh it and retry.");
        }

        SetHeaders(context, analysis);
        return Results.Ok(ToResponse(analysis));
    }

    private static async Task<IResult> ConfirmAsync(
        Guid analysisId,
        ConfirmAnalysisRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        ItemCropService cropService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            entity =>
                entity.Id == analysisId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId,
            cancellationToken);
        if (analysis is null)
        {
            return NotFound(context);
        }
        if (analysis.Status != AnalysisStatus.Completed)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "Only a completed analysis can be confirmed.");
        }

        var detections = await dbContext.AnalysisDetections
            .Where(detection =>
                detection.AnalysisId == analysis.Id
                && detection.TenantId == user.TenantId
                && detection.OwnerObjectId == user.OwnerObjectId)
            .OrderBy(detection => detection.CreatedAt)
            .ThenBy(detection => detection.Id)
            .ToListAsync(cancellationToken);
        var existingItemIds = detections
            .Where(detection => detection.ResultingItemId is not null)
            .Select(detection => detection.ResultingItemId!.Value)
            .ToArray();
        if (detections.All(detection =>
                detection.ReviewStatus != DetectionReviewStatus.Pending))
        {
            SetHeaders(context, analysis);
            return Results.Ok(ToResponse(
                analysis,
                existingItemIds,
                detections.Select(ToResponse).ToArray()));
        }

        var decisions = request.Detections ?? [];
        if (decisions.Count != detections.Count
            || decisions.Select(decision => decision.Id).Distinct().Count()
                != detections.Count)
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "A review decision is required for every detection.");
        }

        var decisionsById = decisions.ToDictionary(decision => decision.Id);
        if (detections.Any(detection => !decisionsById.ContainsKey(detection.Id)))
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "The review contains an unknown or missing detection.");
        }

        var containerIds = decisions
            .Where(decision => decision.Accepted && decision.ContainerId is not null)
            .Select(decision => decision.ContainerId!.Value)
            .ToHashSet();
        var visibleContainerIds = await dbContext.Containers
            .Where(container =>
                container.TenantId == user.TenantId
                && container.OwnerObjectId == user.OwnerObjectId
                && container.DeletedAt == null
                && containerIds.Contains(container.Id))
            .Select(container => container.Id)
            .ToHashSetAsync(cancellationToken);
        if (!containerIds.SetEquals(visibleContainerIds))
        {
            return Problem(
                context,
                StatusCodes.Status400BadRequest,
                "A selected container is unavailable.");
        }

        Dictionary<Guid, NormalizedBoundingBox?> reviewedBoundingBoxes;
        try
        {
            reviewedBoundingBoxes = decisions.ToDictionary(
                decision => decision.Id,
                decision => decision.Accepted
                    ? ToBoundingBox(decision.BoundingBox)
                    : null);
        }
        catch (DomainException exception)
        {
            return Problem(
                context,
                StatusCodes.Status422UnprocessableEntity,
                exception.Message);
        }

        var itemIds = decisions
            .Where(decision => decision.Accepted)
            .ToDictionary(decision => decision.Id, _ => Guid.NewGuid());
        var cropRequests = itemIds
            .Where(entry => reviewedBoundingBoxes[entry.Key] is not null)
            .Select(entry => new ItemCropRequest(
                entry.Value,
                reviewedBoundingBoxes[entry.Key]!))
            .ToArray();
        Photo? sourcePhoto = null;
        if (cropRequests.Length > 0)
        {
            sourcePhoto = await dbContext.Photos.SingleOrDefaultAsync(
                photo =>
                    photo.Id == analysis.PhotoId
                    && photo.TenantId == user.TenantId
                    && photo.OwnerObjectId == user.OwnerObjectId
                    && photo.DeletedAt == null,
                cancellationToken);
            if (sourcePhoto is null)
            {
                return Problem(
                    context,
                    StatusCodes.Status409Conflict,
                    "The source photo is unavailable.");
            }
        }

        await using var transaction = dbContext.Database.IsRelational()
            ? await dbContext.Database.BeginTransactionAsync(cancellationToken)
            : null;
        var now = DateTimeOffset.UtcNow;
        var createdItems = new List<Item>();
        IReadOnlyList<ItemCropResult> createdCrops = [];
        try
        {
            if (sourcePhoto is not null)
            {
                createdCrops = await cropService.CreateAsync(
                    sourcePhoto,
                    cropRequests,
                    cancellationToken);
            }
            var cropsByItemId = createdCrops.ToDictionary(crop => crop.ItemId);

            foreach (var detection in detections)
            {
                var decision = decisionsById[detection.Id];
                if (!decision.Accepted)
                {
                    detection.Reject(now);
                    continue;
                }

                var item = new Item(
                    itemIds[detection.Id],
                    user.TenantId,
                    user.OwnerObjectId,
                    analysis.PhotoId,
                    analysis.Id,
                    decision.Name ?? string.Empty,
                    decision.Description,
                    decision.Category,
                    decision.Quantity ?? 0,
                    detection.Confidence,
                    $"detection:{detection.Id:N}",
                    now);
                if (cropsByItemId.TryGetValue(item.Id, out var crop))
                {
                    item.AttachCrop(
                        crop.BlobName,
                        crop.BoundingBox,
                        crop.PixelWidth,
                        crop.PixelHeight,
                        crop.ContentType,
                        crop.ContentLength,
                        crop.Sha256);
                }
                var assignment = decision.ContainerId is Guid containerId
                    ? ItemAssignment.Confirmed(
                        Guid.NewGuid(),
                        user.TenantId,
                        user.OwnerObjectId,
                        item.Id,
                        containerId,
                        AssignmentSource.User,
                        now)
                    : ItemAssignment.Unassigned(
                        Guid.NewGuid(),
                        user.TenantId,
                        user.OwnerObjectId,
                        item.Id,
                        now);
                item.SetAssignment(assignment);
                detection.Accept(
                    decision.Name ?? string.Empty,
                    decision.Description,
                    decision.Category,
                    decision.Quantity ?? 0,
                    decision.ContainerId,
                    reviewedBoundingBoxes[detection.Id],
                    item.Id,
                    now);
                createdItems.Add(item);
            }

            dbContext.Items.AddRange(createdItems);
            await dbContext.SaveChangesAsync(cancellationToken);
            if (transaction is not null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }
        catch (DomainException exception)
        {
            await cropService.DeleteCreatedAsync(
                createdCrops,
                CancellationToken.None);
            return Problem(
                context,
                StatusCodes.Status422UnprocessableEntity,
                exception.Message);
        }
        catch (Exception exception) when (
            exception is RequestFailedException
                or FileNotFoundException
                or InvalidOperationException
                or DbUpdateException)
        {
            await cropService.DeleteCreatedAsync(
                createdCrops,
                CancellationToken.None);
            return Problem(
                context,
                StatusCodes.Status503ServiceUnavailable,
                "The confirmed inventory could not be persisted.");
        }

        SetHeaders(context, analysis);
        return Results.Ok(ToResponse(
            analysis,
            createdItems.Select(item => item.Id).ToArray(),
            detections.Select(ToResponse).ToArray()));
    }

    private static IResult Accepted(HttpContext context, Analysis analysis)
    {
        SetHeaders(context, analysis);
        return Results.Accepted(
            $"/api/v1/analyses/{analysis.Id}",
            ToResponse(analysis));
    }

    internal static AnalysisResponse ToResponse(
        Analysis analysis,
        IReadOnlyList<Guid>? itemIds = null,
        IReadOnlyList<AnalysisDetectionResponse>? detections = null)
    {
        return new(
            analysis.Id,
            analysis.PhotoId,
            analysis.Status,
            itemIds ?? [],
            detections ?? [],
            analysis.ConfirmedContainerId,
            analysis.Warnings,
            analysis.ErrorCode,
            analysis.ErrorMessage,
            analysis.CorrelationId,
            analysis.StartedAt,
            analysis.CompletedAt,
            analysis.CancellationRequestedAt,
            analysis.CancelledAt,
            analysis.CreatedAt,
            analysis.UpdatedAt);
    }

    internal static AnalysisDetectionResponse ToResponse(
        AnalysisDetection detection)
    {
        return new(
            detection.Id,
            detection.Name,
            detection.Description,
            detection.Category,
            detection.Quantity,
            detection.Confidence,
            detection.SuggestedContainerId,
            ToResponse(detection.GetPredictedBoundingBox()),
            ToResponse(detection.GetReviewedBoundingBox()),
            detection.ReviewStatus,
            detection.ReviewedName,
            detection.ReviewedDescription,
            detection.ReviewedCategory,
            detection.ReviewedQuantity,
            detection.SelectedContainerId,
            detection.ResultingItemId);
    }

    private static NormalizedBoundingBox? ToBoundingBox(
        BoundingBoxRequest? boundingBox) =>
        boundingBox is null
            ? null
            : new(
                boundingBox.X,
                boundingBox.Y,
                boundingBox.Width,
                boundingBox.Height);

    private static BoundingBoxResponse? ToResponse(
        NormalizedBoundingBox? boundingBox) =>
        boundingBox is null
            ? null
            : new(
                boundingBox.X,
                boundingBox.Y,
                boundingBox.Width,
                boundingBox.Height);

    private static bool MatchesEtag(
        HttpContext context,
        Guid concurrencyToken,
        out IResult? error)
    {
        var value = context.Request.Headers.IfMatch.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(value))
        {
            error = Problem(
                context,
                StatusCodes.Status428PreconditionRequired,
                "If-Match is required.");
            return false;
        }
        if (!Guid.TryParse(value.Trim().Trim('"'), out var requestedToken)
            || requestedToken != concurrencyToken)
        {
            error = Problem(
                context,
                StatusCodes.Status412PreconditionFailed,
                "The resource has changed. Refresh it and retry.");
            return false;
        }

        error = null;
        return true;
    }

    private static void SetHeaders(HttpContext context, Analysis analysis)
    {
        context.Response.Headers.ETag = $"\"{analysis.ConcurrencyToken:D}\"";
        context.Response.Headers.Location = $"/api/v1/analyses/{analysis.Id}";
    }

    private static IResult NotFound(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status404NotFound,
            "The requested resource does not exist or is not visible to the caller.");
    }

    private static string GetConfiguredValue(
        IConfiguration configuration,
        string key,
        string fallback)
    {
        var value = configuration[key];
        return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
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
}

using System.Security.Cryptography;
using System.Text;
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
    }

    private static async Task<IResult> CreateAsync(
        Guid photoId,
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

        var promptVersion = configuration[
            "Analysis:PromptVersion"] ?? "2026-09-15.m1";
        var schemaVersion = configuration[
            "Analysis:SchemaVersion"] ?? "item-organizer.analysis-result.v1";
        var model = configuration["Analysis:Model"] ?? "deterministic-mock";
        var applicationVersion = typeof(Program).Assembly
            .GetName().Version?.ToString() ?? "development";
        var requestHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(
                    $"{photoId:D}|{promptVersion}|{schemaVersion}|{model}")))
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
            null,
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

    private static IResult Accepted(HttpContext context, Analysis analysis)
    {
        SetHeaders(context, analysis);
        return Results.Accepted(
            $"/api/v1/analyses/{analysis.Id}",
            ToResponse(analysis));
    }

    internal static AnalysisResponse ToResponse(
        Analysis analysis,
        IReadOnlyList<Guid>? itemIds = null)
    {
        return new(
            analysis.Id,
            analysis.PhotoId,
            analysis.Status,
            itemIds ?? [],
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

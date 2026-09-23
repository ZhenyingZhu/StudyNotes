using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Api;

public static class ReadEndpoints
{
    public static void MapReadEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1");

        api.MapGet("/health/live", () => Results.Ok(new { status = "healthy" }))
            .AllowAnonymous();
        api.MapGet("/health/ready", ReadyAsync).AllowAnonymous();

        var reads = api.MapGroup(string.Empty)
            .RequireAuthorization(AuthorizationPolicies.Read);

        reads.MapGet("/summary", SummaryAsync);
        reads.MapGet("/containers", ListContainersAsync);
        reads.MapGet("/containers/{containerId:guid}", GetContainerAsync);
        reads.MapGet("/containers/{containerId:guid}/items", ListContainerItemsAsync);
        reads.MapGet("/photos", ListPhotosAsync);
        reads.MapGet("/photos/{photoId:guid}", GetPhotoAsync);
        reads.MapGet("/photos/{photoId:guid}/content", PhotoContentUnavailable);
        reads.MapGet("/analyses/{analysisId:guid}", GetAnalysisAsync);
        reads.MapGet("/items", ListItemsAsync);
        reads.MapGet("/items/{itemId:guid}", GetItemAsync);
    }

    private static async Task<IResult> ReadyAsync(
        ItemOrganizerDbContext dbContext,
        ILoggerFactory loggerFactory,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            if (await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return Results.Ok(new { status = "healthy" });
            }
        }
        catch (Exception exception)
        {
            loggerFactory.CreateLogger("Health.Readiness").LogError(
                exception,
                "Database readiness check failed. Correlation ID: {CorrelationId}",
                context.TraceIdentifier);
        }

        return Problem(
            context,
            StatusCodes.Status503ServiceUnavailable,
            "A required dependency is unavailable.");
    }

    private static async Task<Ok<SummaryResponse>> SummaryAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var containers = dbContext.Containers.Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId
            && entity.DeletedAt == null);
        var photos = dbContext.Photos.Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId
            && entity.DeletedAt == null);
        var items = dbContext.Items.Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId
            && entity.DeletedAt == null);
        var analyses = dbContext.Analyses.Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId);

        var response = new SummaryResponse(
            await containers.CountAsync(cancellationToken),
            await photos.CountAsync(cancellationToken),
            await items.CountAsync(cancellationToken),
            await analyses.CountAsync(
                entity => entity.Status == AnalysisStatus.Queued
                    || entity.Status == AnalysisStatus.Running,
                cancellationToken),
            await items.CountAsync(
                entity => entity.Assignment.Status == AssignmentStatus.Unassigned,
                cancellationToken));
        return TypedResults.Ok(response);
    }

    private static async Task<IResult> ListContainersAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        string? search,
        string? location,
        int? pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        if (!TryGetPage(context, pageSize, continuationToken, out var page, out var error))
        {
            return error!;
        }

        var user = currentUserAccessor.GetRequired();
        var query = dbContext.Containers
            .AsNoTracking()
            .Where(entity =>
                entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null);
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(entity =>
                EF.Functions.ILike(entity.Name, $"%{search.Trim()}%")
                || (entity.Description != null
                    && EF.Functions.ILike(entity.Description, $"%{search.Trim()}%")));
        }
        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(entity =>
                entity.Location != null
                && EF.Functions.ILike(entity.Location, location.Trim()));
        }

        var rows = await query
            .OrderBy(entity => entity.CreatedAt)
            .ThenBy(entity => entity.Id)
            .Skip(page!.Offset)
            .Take(page.PageSize + 1)
            .Select(entity => new ContainerResponse(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.Location,
                entity.Labels,
                dbContext.Items.Count(item =>
                    item.Assignment.ContainerId == entity.Id
                    && item.Assignment.Status == AssignmentStatus.Confirmed
                    && item.DeletedAt == null),
                entity.CreatedAt,
                entity.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Results.Ok(ToPage(rows, page));
    }

    private static async Task<IResult> GetContainerAsync(
        Guid containerId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await dbContext.Containers
            .AsNoTracking()
            .Where(container =>
                container.Id == containerId
                && container.TenantId == user.TenantId
                && container.OwnerObjectId == user.OwnerObjectId
                && container.DeletedAt == null)
            .Select(container => new
            {
                Response = new ContainerResponse(
                    container.Id,
                    container.Name,
                    container.Description,
                    container.Location,
                    container.Labels,
                    dbContext.Items.Count(item =>
                        item.Assignment.ContainerId == container.Id
                        && item.Assignment.Status == AssignmentStatus.Confirmed
                        && item.DeletedAt == null),
                    container.CreatedAt,
                    container.UpdatedAt),
                container.ConcurrencyToken
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            return NotFound(context);
        }

        SetEtag(context, entity.ConcurrencyToken);
        return Results.Ok(entity.Response);
    }

    private static async Task<IResult> ListContainerItemsAsync(
        Guid containerId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        int? pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var containerExists = await dbContext.Containers.AnyAsync(
            entity =>
                entity.Id == containerId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null,
            cancellationToken);
        if (!containerExists)
        {
            return NotFound(context);
        }

        return await ListItemsCoreAsync(
            dbContext,
            user,
            context,
            null,
            null,
            containerId,
            null,
            null,
            pageSize,
            continuationToken,
            cancellationToken);
    }

    private static async Task<IResult> ListPhotosAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        AnalysisStatus? status,
        DateTimeOffset? uploadedFrom,
        DateTimeOffset? uploadedTo,
        int? pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        if (!TryGetPage(context, pageSize, continuationToken, out var page, out var error))
        {
            return error!;
        }
        if (uploadedFrom > uploadedTo)
        {
            return ValidationProblem(
                context,
                "uploadedFrom must not be later than uploadedTo.");
        }

        var user = currentUserAccessor.GetRequired();
        var query = dbContext.Photos.AsNoTracking().Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId
            && entity.DeletedAt == null);
        if (uploadedFrom is not null)
        {
            query = query.Where(entity => entity.CreatedAt >= uploadedFrom);
        }
        if (uploadedTo is not null)
        {
            query = query.Where(entity => entity.CreatedAt <= uploadedTo);
        }
        if (status is not null)
        {
            query = query.Where(photo => dbContext.Analyses.Any(analysis =>
                analysis.PhotoId == photo.Id && analysis.Status == status));
        }

        var rows = await query
            .OrderBy(entity => entity.CreatedAt)
            .ThenBy(entity => entity.Id)
            .Skip(page!.Offset)
            .Take(page.PageSize + 1)
            .Select(photo => new PhotoResponse(
                photo.Id,
                photo.ContentType,
                photo.ContentLength,
                photo.Width,
                photo.Height,
                photo.RetentionState,
                dbContext.Analyses
                    .Where(analysis => analysis.PhotoId == photo.Id)
                    .OrderByDescending(analysis => analysis.CreatedAt)
                    .Select(analysis => (AnalysisStatus?)analysis.Status)
                    .FirstOrDefault(),
                photo.RetainUntil,
                photo.CreatedAt,
                photo.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Results.Ok(ToPage(rows, page));
    }

    private static async Task<IResult> GetPhotoAsync(
        Guid photoId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await dbContext.Photos
            .AsNoTracking()
            .Where(photo =>
                photo.Id == photoId
                && photo.TenantId == user.TenantId
                && photo.OwnerObjectId == user.OwnerObjectId
                && photo.DeletedAt == null)
            .Select(photo => new
            {
                Response = new PhotoResponse(
                    photo.Id,
                    photo.ContentType,
                    photo.ContentLength,
                    photo.Width,
                    photo.Height,
                    photo.RetentionState,
                    dbContext.Analyses
                        .Where(analysis => analysis.PhotoId == photo.Id)
                        .OrderByDescending(analysis => analysis.CreatedAt)
                        .Select(analysis => (AnalysisStatus?)analysis.Status)
                        .FirstOrDefault(),
                    photo.RetainUntil,
                    photo.CreatedAt,
                    photo.UpdatedAt),
                photo.ConcurrencyToken
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }

        SetEtag(context, entity.ConcurrencyToken);
        return Results.Ok(entity.Response);
    }

    private static async Task<IResult> GetAnalysisAsync(
        Guid analysisId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await dbContext.Analyses
            .AsNoTracking()
            .Where(analysis =>
                analysis.Id == analysisId
                && analysis.TenantId == user.TenantId
                && analysis.OwnerObjectId == user.OwnerObjectId)
            .Select(analysis => new
            {
                Response = new AnalysisResponse(
                    analysis.Id,
                    analysis.PhotoId,
                    analysis.Status,
                    dbContext.Items
                        .Where(item =>
                            item.AnalysisId == analysis.Id && item.DeletedAt == null)
                        .OrderBy(item => item.CreatedAt)
                        .Select(item => item.Id)
                        .ToArray(),
                    analysis.Warnings,
                    analysis.ErrorCode,
                    analysis.ErrorMessage,
                    analysis.CorrelationId,
                    analysis.StartedAt,
                    analysis.CompletedAt,
                    analysis.CancellationRequestedAt,
                    analysis.CancelledAt,
                    analysis.CreatedAt,
                    analysis.UpdatedAt),
                analysis.ConcurrencyToken
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }

        SetEtag(context, entity.ConcurrencyToken);
        return Results.Ok(entity.Response);
    }

    private static Task<IResult> ListItemsAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        string? search,
        string? category,
        Guid? containerId,
        Guid? photoId,
        AssignmentStatus? assignmentStatus,
        int? pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        return ListItemsCoreAsync(
            dbContext,
            currentUserAccessor.GetRequired(),
            context,
            search,
            category,
            containerId,
            photoId,
            assignmentStatus,
            pageSize,
            continuationToken,
            cancellationToken);
    }

    private static async Task<IResult> ListItemsCoreAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUser user,
        HttpContext context,
        string? search,
        string? category,
        Guid? containerId,
        Guid? photoId,
        AssignmentStatus? assignmentStatus,
        int? pageSize,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        if (!TryGetPage(context, pageSize, continuationToken, out var page, out var error))
        {
            return error!;
        }

        var query = dbContext.Items.AsNoTracking().Where(entity =>
            entity.TenantId == user.TenantId
            && entity.OwnerObjectId == user.OwnerObjectId
            && entity.DeletedAt == null);
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(entity =>
                EF.Functions.ILike(entity.Name, $"%{search.Trim()}%")
                || (entity.Description != null
                    && EF.Functions.ILike(entity.Description, $"%{search.Trim()}%")));
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(entity =>
                entity.Category != null
                && EF.Functions.ILike(entity.Category, category.Trim()));
        }
        if (containerId is not null)
        {
            query = query.Where(entity =>
                entity.Assignment.Status == AssignmentStatus.Confirmed
                && entity.Assignment.ContainerId == containerId);
        }
        if (photoId is not null)
        {
            query = query.Where(entity => entity.PhotoId == photoId);
        }
        if (assignmentStatus is not null)
        {
            query = query.Where(entity => entity.Assignment.Status == assignmentStatus);
        }

        var rows = await query
            .OrderBy(entity => entity.CreatedAt)
            .ThenBy(entity => entity.Id)
            .Skip(page!.Offset)
            .Take(page.PageSize + 1)
            .Select(ToItemResponse())
            .ToListAsync(cancellationToken);
        return Results.Ok(ToPage(rows, page));
    }

    private static async Task<IResult> GetItemAsync(
        Guid itemId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await dbContext.Items
            .AsNoTracking()
            .Where(item =>
                item.Id == itemId
                && item.TenantId == user.TenantId
                && item.OwnerObjectId == user.OwnerObjectId
                && item.DeletedAt == null)
            .Select(item => new
            {
                Response = new ItemResponse(
                    item.Id,
                    item.Name,
                    item.Description,
                    item.Category,
                    item.Quantity,
                    item.Confidence,
                    item.PhotoId,
                    item.AnalysisId,
                    item.Assignment.ContainerId,
                    item.Assignment.SuggestedContainerId,
                    item.Assignment.Status,
                    item.CreatedAt,
                    item.UpdatedAt),
                item.ConcurrencyToken
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }

        SetEtag(context, entity.ConcurrencyToken);
        return Results.Ok(entity.Response);
    }

    private static IResult PhotoContentUnavailable(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status503ServiceUnavailable,
            "Private photo content access is not configured.");
    }

    private static System.Linq.Expressions.Expression<Func<Item, ItemResponse>>
        ToItemResponse()
    {
        return item => new ItemResponse(
            item.Id,
            item.Name,
            item.Description,
            item.Category,
            item.Quantity,
            item.Confidence,
            item.PhotoId,
            item.AnalysisId,
            item.Assignment.ContainerId,
            item.Assignment.SuggestedContainerId,
            item.Assignment.Status,
            item.CreatedAt,
            item.UpdatedAt);
    }

    private static PageResponse<T> ToPage<T>(List<T> rows, PageRequest page)
    {
        var hasMore = rows.Count > page.PageSize;
        if (hasMore)
        {
            rows.RemoveAt(rows.Count - 1);
        }

        return new(rows, hasMore ? page.NextToken() : null);
    }

    private static bool TryGetPage(
        HttpContext context,
        int? pageSize,
        string? continuationToken,
        out PageRequest? page,
        out IResult? error)
    {
        if (PageRequest.TryCreate(
                pageSize,
                continuationToken,
                out page,
                out var message))
        {
            error = null;
            return true;
        }

        error = ValidationProblem(context, message!);
        return false;
    }

    private static IResult ValidationProblem(HttpContext context, string detail)
    {
        return Problem(
            context,
            StatusCodes.Status400BadRequest,
            detail,
            "Invalid request.");
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
        string detail,
        string? title = null)
    {
        return Results.Problem(
            statusCode: statusCode,
            title: title,
            detail: detail,
            type: $"https://httpstatuses.com/{statusCode}",
            extensions: new Dictionary<string, object?>
            {
                ["correlationId"] = context.TraceIdentifier
            });
    }

    private static void SetEtag(HttpContext context, Guid concurrencyToken)
    {
        context.Response.Headers.ETag = $"\"{concurrencyToken:D}\"";
    }
}

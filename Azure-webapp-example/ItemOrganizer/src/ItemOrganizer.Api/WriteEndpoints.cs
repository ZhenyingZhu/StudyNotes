using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Api;

public static class WriteEndpoints
{
    public static void MapWriteEndpoints(this WebApplication app)
    {
        var writes = app.MapGroup("/api/v1")
            .RequireAuthorization(AuthorizationPolicies.Write);

        writes.MapPost("/containers", CreateContainerAsync);
        writes.MapPatch("/containers/{containerId:guid}", UpdateContainerAsync);
        writes.MapDelete("/containers/{containerId:guid}", DeleteContainerAsync);
        writes.MapPost("/items", CreateItemAsync);
        writes.MapPatch("/items/{itemId:guid}", UpdateItemAsync);
        writes.MapDelete("/items/{itemId:guid}", DeleteItemAsync);
        writes.MapPut("/items/{itemId:guid}/container", AssignItemAsync);
        writes.MapDelete("/items/{itemId:guid}/container", UnassignItemAsync);
    }

    private static async Task<IResult> CreateContainerAsync(
        ContainerRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var normalizedName = request.Name?.Trim().ToUpperInvariant();
        if (!string.IsNullOrWhiteSpace(normalizedName)
            && await dbContext.Containers.AnyAsync(
                entity =>
                    entity.TenantId == user.TenantId
                    && entity.OwnerObjectId == user.OwnerObjectId
                    && entity.NormalizedName == normalizedName
                    && entity.DeletedAt == null,
                cancellationToken))
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "A container with this name already exists.");
        }

        try
        {
            var entity = new StorageContainer(
                Guid.NewGuid(),
                user.TenantId,
                user.OwnerObjectId,
                request.Name ?? string.Empty,
                request.Description,
                request.Location,
                request.Labels,
                DateTimeOffset.UtcNow);
            dbContext.Containers.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            SetEtag(context, entity.ConcurrencyToken);
            return Results.Created(
                $"/api/v1/containers/{entity.Id}",
                ToContainerResponse(entity, 0));
        }
        catch (DomainException exception)
        {
            return Unprocessable(context, exception.Message);
        }
    }

    private static async Task<IResult> UpdateContainerAsync(
        Guid containerId,
        ContainerRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindContainerAsync(
            dbContext,
            user,
            containerId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        var normalizedName = request.Name?.Trim().ToUpperInvariant();
        if (!string.IsNullOrWhiteSpace(normalizedName)
            && await dbContext.Containers.AnyAsync(
                candidate =>
                    candidate.Id != entity.Id
                    && candidate.TenantId == user.TenantId
                    && candidate.OwnerObjectId == user.OwnerObjectId
                    && candidate.NormalizedName == normalizedName
                    && candidate.DeletedAt == null,
                cancellationToken))
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "A container with this name already exists.");
        }

        try
        {
            entity.Update(
                request.Name ?? string.Empty,
                request.Description,
                request.Location,
                request.Labels,
                DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
            var itemCount = await CountContainerItemsAsync(
                dbContext,
                entity.Id,
                cancellationToken);
            SetEtag(context, entity.ConcurrencyToken);
            return Results.Ok(ToContainerResponse(entity, itemCount));
        }
        catch (DomainException exception)
        {
            return Unprocessable(context, exception.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Stale(context);
        }
    }

    private static async Task<IResult> DeleteContainerAsync(
        Guid containerId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindContainerAsync(
            dbContext,
            user,
            containerId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        var containsItems = await dbContext.Items.AnyAsync(
            item =>
                item.TenantId == user.TenantId
                && item.OwnerObjectId == user.OwnerObjectId
                && item.DeletedAt == null
                && item.Assignment.Status == AssignmentStatus.Confirmed
                && item.Assignment.ContainerId == entity.Id,
            cancellationToken);
        if (containsItems)
        {
            return Problem(
                context,
                StatusCodes.Status409Conflict,
                "A non-empty container cannot be deleted.");
        }

        entity.MarkDeleted(false, DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateItemAsync(
        ItemRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        try
        {
            var now = DateTimeOffset.UtcNow;
            var entity = Item.CreateManual(
                Guid.NewGuid(),
                user.TenantId,
                user.OwnerObjectId,
                request.Name,
                request.Description,
                request.Category,
                request.Quantity,
                now);
            entity.SetAssignment(ItemAssignment.Unassigned(
                Guid.NewGuid(),
                user.TenantId,
                user.OwnerObjectId,
                entity.Id,
                now));
            dbContext.Items.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            SetEtag(context, entity.ConcurrencyToken);
            return Results.Created(
                $"/api/v1/items/{entity.Id}",
                ToItemResponse(entity));
        }
        catch (DomainException exception)
        {
            return Unprocessable(context, exception.Message);
        }
    }

    private static async Task<IResult> UpdateItemAsync(
        Guid itemId,
        ItemRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindItemAsync(
            dbContext,
            user,
            itemId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        try
        {
            entity.Update(
                request.Name,
                request.Description,
                request.Category,
                request.Quantity,
                DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
            SetEtag(context, entity.ConcurrencyToken);
            return Results.Ok(ToItemResponse(entity));
        }
        catch (DomainException exception)
        {
            return Unprocessable(context, exception.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Stale(context);
        }
    }

    private static async Task<IResult> DeleteItemAsync(
        Guid itemId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindItemAsync(
            dbContext,
            user,
            itemId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        entity.MarkDeleted(DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> AssignItemAsync(
        Guid itemId,
        AssignmentRequest request,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindItemAsync(
            dbContext,
            user,
            itemId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        var containerExists = await dbContext.Containers.AnyAsync(
            container =>
                container.Id == request.ContainerId
                && container.TenantId == user.TenantId
                && container.OwnerObjectId == user.OwnerObjectId
                && container.DeletedAt == null,
            cancellationToken);
        if (!containerExists)
        {
            return NotFound(context);
        }

        try
        {
            if (request.AcceptSuggestion)
            {
                entity.Assignment.AcceptSuggestion(
                    request.ContainerId,
                    DateTimeOffset.UtcNow);
            }
            else
            {
                entity.Assignment.Move(
                    request.ContainerId,
                    DateTimeOffset.UtcNow);
            }
            entity.Update(
                entity.Name,
                entity.Description,
                entity.Category,
                entity.Quantity,
                DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync(cancellationToken);
            SetEtag(context, entity.ConcurrencyToken);
            return Results.Ok(ToItemResponse(entity));
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
            return Stale(context);
        }
    }

    private static async Task<IResult> UnassignItemAsync(
        Guid itemId,
        ItemOrganizerDbContext dbContext,
        CurrentUserAccessor currentUserAccessor,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var user = currentUserAccessor.GetRequired();
        var entity = await FindItemAsync(
            dbContext,
            user,
            itemId,
            cancellationToken);
        if (entity is null)
        {
            return NotFound(context);
        }
        if (!MatchesEtag(context, entity.ConcurrencyToken, out var error))
        {
            return error!;
        }

        entity.Assignment.Remove(DateTimeOffset.UtcNow);
        entity.Update(
            entity.Name,
            entity.Description,
            entity.Category,
            entity.Quantity,
            DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetEtag(context, entity.ConcurrencyToken);
        return Results.Ok(ToItemResponse(entity));
    }

    private static Task<StorageContainer?> FindContainerAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUser user,
        Guid containerId,
        CancellationToken cancellationToken)
    {
        return dbContext.Containers.SingleOrDefaultAsync(
            entity =>
                entity.Id == containerId
                && entity.TenantId == user.TenantId
                && entity.OwnerObjectId == user.OwnerObjectId
                && entity.DeletedAt == null,
            cancellationToken);
    }

    private static Task<Item?> FindItemAsync(
        ItemOrganizerDbContext dbContext,
        CurrentUser user,
        Guid itemId,
        CancellationToken cancellationToken)
    {
        return dbContext.Items
            .Include(entity => entity.Assignment)
            .SingleOrDefaultAsync(
                entity =>
                    entity.Id == itemId
                    && entity.TenantId == user.TenantId
                    && entity.OwnerObjectId == user.OwnerObjectId
                    && entity.DeletedAt == null,
                cancellationToken);
    }

    private static Task<int> CountContainerItemsAsync(
        ItemOrganizerDbContext dbContext,
        Guid containerId,
        CancellationToken cancellationToken)
    {
        return dbContext.Items.CountAsync(
            item =>
                item.DeletedAt == null
                && item.Assignment.Status == AssignmentStatus.Confirmed
                && item.Assignment.ContainerId == containerId,
            cancellationToken);
    }

    private static ContainerResponse ToContainerResponse(
        StorageContainer entity,
        int itemCount)
    {
        return new(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Location,
            entity.Labels,
            itemCount,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    private static ItemResponse ToItemResponse(Item entity)
    {
        return new(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Category,
            entity.Quantity,
            entity.Confidence,
            entity.PhotoId,
            entity.AnalysisId,
            entity.Assignment.ContainerId,
            entity.Assignment.SuggestedContainerId,
            entity.Assignment.Status,
            entity.CreatedAt,
            entity.UpdatedAt);
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
            error = Stale(context);
            return false;
        }

        error = null;
        return true;
    }

    private static IResult NotFound(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status404NotFound,
            "The requested resource does not exist or is not visible to the caller.");
    }

    private static IResult Stale(HttpContext context)
    {
        return Problem(
            context,
            StatusCodes.Status412PreconditionFailed,
            "The resource has changed. Refresh it and retry.");
    }

    private static IResult Unprocessable(HttpContext context, string detail)
    {
        return Problem(
            context,
            StatusCodes.Status422UnprocessableEntity,
            detail);
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

    private static void SetEtag(HttpContext context, Guid concurrencyToken)
    {
        context.Response.Headers.ETag = $"\"{concurrencyToken:D}\"";
    }
}

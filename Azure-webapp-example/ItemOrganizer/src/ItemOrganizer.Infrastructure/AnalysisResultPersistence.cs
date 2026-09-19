using ItemOrganizer.Domain;
using Microsoft.EntityFrameworkCore;

namespace ItemOrganizer.Infrastructure;

public sealed record DetectedItemDraft(
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    decimal Confidence,
    Guid? SuggestedContainerId);

public sealed class AnalysisResultPersistence(ItemOrganizerDbContext dbContext)
{
    public async Task<IReadOnlyList<Item>> PersistCompletedAnalysisAsync(
        Guid analysisId,
        IReadOnlyCollection<DetectedItemDraft> detections,
        DateTimeOffset completedAt,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(
            cancellationToken);

        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            candidate => candidate.Id == analysisId,
            cancellationToken) ?? throw new DomainException("Analysis was not found.");

        if (analysis.Status == AnalysisStatus.Completed)
        {
            return await dbContext.Items
                .Where(item => item.AnalysisId == analysisId)
                .Include(item => item.Assignment)
                .ToListAsync(cancellationToken);
        }

        if (analysis.Status != AnalysisStatus.Running)
        {
            throw new DomainException("Only a running analysis can persist results.");
        }

        var warnings = new List<string>();
        var accepted = new List<NormalizedDetection>();

        foreach (var detection in detections)
        {
            if (detection.Confidence < 0 || detection.Confidence > 1)
            {
                throw new DomainException("Detection confidence must be between 0 and 1.");
            }

            if (detection.Quantity <= 0)
            {
                throw new DomainException("Detection quantity must be positive.");
            }

            if (detection.Confidence < 0.35m)
            {
                warnings.Add($"Discarded low-confidence detection: {detection.Name.Trim()}.");
                continue;
            }

            var normalizedName = NormalizeRequired(detection.Name, "Detection name");
            var normalizedCategory = NormalizeOptional(detection.Category);
            var effectiveSuggestion = detection.Confidence >= 0.80m
                ? detection.SuggestedContainerId
                : null;
            var key = string.Join(
                "|",
                normalizedName,
                normalizedCategory ?? string.Empty,
                effectiveSuggestion?.ToString("N") ?? string.Empty);

            accepted.Add(new(
                detection,
                normalizedName,
                normalizedCategory,
                effectiveSuggestion,
                key));
        }

        var merged = accepted
            .GroupBy(detection => detection.Key, StringComparer.Ordinal)
            .Select(group =>
            {
                var best = group.OrderByDescending(item => item.Draft.Confidence).First();
                return best with
                {
                    Draft = best.Draft with
                    {
                        Quantity = checked(group.Sum(item => item.Draft.Quantity)),
                        Confidence = group.Max(item => item.Draft.Confidence)
                    }
                };
            })
            .ToArray();

        var containerIds = merged
            .Where(detection => detection.SuggestedContainerId is not null)
            .Select(detection => detection.SuggestedContainerId!.Value)
            .ToHashSet();

        if (analysis.ConfirmedContainerId is not null)
        {
            containerIds.Add(analysis.ConfirmedContainerId.Value);
        }

        var visibleContainerIds = await dbContext.Containers
            .Where(container =>
                container.TenantId == analysis.TenantId &&
                container.OwnerObjectId == analysis.OwnerObjectId &&
                container.DeletedAt == null &&
                containerIds.Contains(container.Id))
            .Select(container => container.Id)
            .ToHashSetAsync(cancellationToken);

        if (analysis.ConfirmedContainerId is Guid confirmedContainerId &&
            !visibleContainerIds.Contains(confirmedContainerId))
        {
            throw new DomainException("The confirmed target container is not visible to the owner.");
        }

        var items = new List<Item>(merged.Length);
        foreach (var detection in merged)
        {
            var item = new Item(
                Guid.NewGuid(),
                analysis.TenantId,
                analysis.OwnerObjectId,
                analysis.PhotoId,
                analysis.Id,
                detection.Draft.Name,
                detection.Draft.Description,
                detection.Draft.Category,
                detection.Draft.Quantity,
                detection.Draft.Confidence,
                detection.Key,
                completedAt);

            ItemAssignment assignment;
            if (analysis.ConfirmedContainerId is Guid targetContainerId)
            {
                assignment = ItemAssignment.Confirmed(
                    Guid.NewGuid(),
                    analysis.TenantId,
                    analysis.OwnerObjectId,
                    item.Id,
                    targetContainerId,
                    AssignmentSource.ConvenienceWorkflow,
                    completedAt);
            }
            else if (detection.SuggestedContainerId is Guid suggestedContainerId &&
                     visibleContainerIds.Contains(suggestedContainerId))
            {
                assignment = ItemAssignment.Suggested(
                    Guid.NewGuid(),
                    analysis.TenantId,
                    analysis.OwnerObjectId,
                    item.Id,
                    suggestedContainerId,
                    completedAt);
            }
            else
            {
                if (detection.SuggestedContainerId is not null)
                {
                    warnings.Add(
                        $"Ignored inaccessible container suggestion for {detection.Draft.Name.Trim()}.");
                }

                assignment = ItemAssignment.Unassigned(
                    Guid.NewGuid(),
                    analysis.TenantId,
                    analysis.OwnerObjectId,
                    item.Id,
                    completedAt);
            }

            item.SetAssignment(assignment);
            items.Add(item);
        }

        dbContext.Items.AddRange(items);
        analysis.Complete(warnings, completedAt);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return items;
    }

    private static string NormalizeRequired(string value, string name)
    {
        var normalized = value?.Trim().ToUpperInvariant();
        return string.IsNullOrWhiteSpace(normalized)
            ? throw new DomainException($"{name} is required.")
            : normalized;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim().ToUpperInvariant();
    }

    private sealed record NormalizedDetection(
        DetectedItemDraft Draft,
        string NormalizedName,
        string? NormalizedCategory,
        Guid? SuggestedContainerId,
        string Key);
}

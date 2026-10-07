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
    public async Task<IReadOnlyList<AnalysisDetection>> PersistCompletedAnalysisAsync(
        Guid analysisId,
        IReadOnlyCollection<DetectedItemDraft> detections,
        IReadOnlyCollection<string>? providerWarnings,
        DateTimeOffset completedAt,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = dbContext.Database.IsRelational()
            ? await dbContext.Database.BeginTransactionAsync(cancellationToken)
            : null;

        var analysis = await dbContext.Analyses.SingleOrDefaultAsync(
            candidate => candidate.Id == analysisId,
            cancellationToken) ?? throw new DomainException("Analysis was not found.");

        if (analysis.Status == AnalysisStatus.Completed)
        {
            return await dbContext.AnalysisDetections
                .Where(detection => detection.AnalysisId == analysisId)
                .ToListAsync(cancellationToken);
        }

        if (analysis.Status != AnalysisStatus.Running)
        {
            throw new DomainException("Only a running analysis can persist results.");
        }

        var warnings = providerWarnings?
            .Where(warning => !string.IsNullOrWhiteSpace(warning))
            .ToList() ?? [];
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

        var visibleContainerIds = await dbContext.Containers
            .Where(container =>
                container.TenantId == analysis.TenantId &&
                container.OwnerObjectId == analysis.OwnerObjectId &&
                container.DeletedAt == null &&
                containerIds.Contains(container.Id))
            .Select(container => container.Id)
            .ToHashSetAsync(cancellationToken);

        var drafts = new List<AnalysisDetection>(merged.Length);
        foreach (var detection in merged)
        {
            var suggestedContainerId =
                detection.SuggestedContainerId is Guid suggestion &&
                visibleContainerIds.Contains(suggestion)
                    ? (Guid?)suggestion
                    : null;
            if (detection.SuggestedContainerId is not null &&
                suggestedContainerId is null)
            {
                warnings.Add(
                    $"Ignored inaccessible container suggestion for {detection.Draft.Name.Trim()}.");
            }

            drafts.Add(new AnalysisDetection(
                Guid.NewGuid(),
                analysis.TenantId,
                analysis.OwnerObjectId,
                analysis.Id,
                detection.Draft.Name,
                detection.Draft.Description,
                detection.Draft.Category,
                detection.Draft.Quantity,
                detection.Draft.Confidence,
                suggestedContainerId,
                completedAt));
        }

        dbContext.AnalysisDetections.AddRange(drafts);
        analysis.Complete(warnings, completedAt);
        await dbContext.SaveChangesAsync(cancellationToken);
        if (transaction is not null)
        {
            await transaction.CommitAsync(cancellationToken);
        }

        return drafts;
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

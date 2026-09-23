using ItemOrganizer.Domain;

namespace ItemOrganizer.Api;

public sealed record PageResponse<T>(
    IReadOnlyList<T> Items,
    string? ContinuationToken);

public sealed record SummaryResponse(
    int Containers,
    int Photos,
    int Items,
    int AnalysesInProgress,
    int UnassignedItems);

public sealed record ContainerResponse(
    Guid Id,
    string Name,
    string? Description,
    string? Location,
    IReadOnlyList<string> Labels,
    int ItemCount,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record PhotoResponse(
    Guid Id,
    string ContentType,
    long ContentLength,
    int Width,
    int Height,
    PhotoRetentionState RetentionState,
    AnalysisStatus? AnalysisStatus,
    DateTimeOffset RetainUntil,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record AnalysisResponse(
    Guid Id,
    Guid PhotoId,
    AnalysisStatus Status,
    IReadOnlyList<Guid> ItemIds,
    IReadOnlyList<string> Warnings,
    string? ErrorCode,
    string? ErrorMessage,
    string? CorrelationId,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    DateTimeOffset? CancellationRequestedAt,
    DateTimeOffset? CancelledAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record ItemResponse(
    Guid Id,
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    decimal Confidence,
    Guid PhotoId,
    Guid AnalysisId,
    Guid? ContainerId,
    Guid? SuggestedContainerId,
    AssignmentStatus AssignmentStatus,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

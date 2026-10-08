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
    IReadOnlyList<AnalysisDetectionResponse> Detections,
    Guid? DefaultContainerId,
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

public sealed record AnalysisDetectionResponse(
    Guid Id,
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    decimal Confidence,
    Guid? SuggestedContainerId,
    BoundingBoxResponse? PredictedBoundingBox,
    BoundingBoxResponse? ReviewedBoundingBox,
    DetectionReviewStatus ReviewStatus,
    string? ReviewedName,
    string? ReviewedDescription,
    string? ReviewedCategory,
    int? ReviewedQuantity,
    Guid? SelectedContainerId,
    Guid? ResultingItemId);

public sealed record ConfirmAnalysisRequest(
    IReadOnlyList<DetectionReviewRequest> Detections);

public sealed record DetectionReviewRequest(
    Guid Id,
    bool Accepted,
    string? Name,
    string? Description,
    string? Category,
    int? Quantity,
    Guid? ContainerId,
    BoundingBoxRequest? BoundingBox);

public sealed record BoundingBoxRequest(
    decimal X,
    decimal Y,
    decimal Width,
    decimal Height);

public sealed record BoundingBoxResponse(
    decimal X,
    decimal Y,
    decimal Width,
    decimal Height);

public sealed record ItemResponse(
    Guid Id,
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    decimal? Confidence,
    Guid? PhotoId,
    Guid? AnalysisId,
    Guid? ContainerId,
    Guid? SuggestedContainerId,
    AssignmentStatus AssignmentStatus,
    bool HasCrop,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record ContainerRequest(
    string Name,
    string? Description,
    string? Location,
    IReadOnlyList<string>? Labels);

public sealed record ItemRequest(
    string Name,
    string? Description,
    string? Category,
    int Quantity);

public sealed record AssignmentRequest(
    Guid ContainerId,
    bool AcceptSuggestion);

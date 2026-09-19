namespace ItemOrganizer.Domain;

public sealed class Analysis : OwnedEntity
{
    public const int MaximumDeliveryAttempts = 6;

    private Analysis()
    {
    }

    public Analysis(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid photoId,
        string promptVersion,
        string schemaVersion,
        string model,
        string applicationVersion,
        Guid? confirmedContainerId,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        PhotoId = RequireId(photoId, nameof(photoId));
        PromptVersion = RequireText(promptVersion, nameof(promptVersion), 100);
        SchemaVersion = RequireText(schemaVersion, nameof(schemaVersion), 100);
        Model = RequireText(model, nameof(model), 100);
        ApplicationVersion = RequireText(applicationVersion, nameof(applicationVersion), 100);
        ConfirmedContainerId = confirmedContainerId;
        Status = AnalysisStatus.Queued;
    }

    public Guid PhotoId { get; private set; }

    public AnalysisStatus Status { get; private set; }

    public int DeliveryAttemptCount { get; private set; }

    public string PromptVersion { get; private set; } = string.Empty;

    public string SchemaVersion { get; private set; } = string.Empty;

    public string Model { get; private set; } = string.Empty;

    public string ApplicationVersion { get; private set; } = string.Empty;

    public Guid? ConfirmedContainerId { get; private set; }

    public string? ErrorCode { get; private set; }

    public string? ErrorMessage { get; private set; }

    public string? CorrelationId { get; private set; }

    public string[] Warnings { get; private set; } = [];

    public DateTimeOffset? StartedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public DateTimeOffset? CancellationRequestedAt { get; private set; }

    public DateTimeOffset? CancelledAt { get; private set; }

    public void Start(DateTimeOffset startedAt)
    {
        EnsureStatus(AnalysisStatus.Queued);
        DeliveryAttemptCount++;
        if (DeliveryAttemptCount > MaximumDeliveryAttempts)
        {
            throw new DomainException("Analysis delivery attempt limit has been exceeded.");
        }

        Status = AnalysisStatus.Running;
        StartedAt ??= startedAt;
        Touch(startedAt);
    }

    public void RequestCancellation(DateTimeOffset requestedAt)
    {
        if (Status == AnalysisStatus.Queued)
        {
            Status = AnalysisStatus.Cancelled;
            CancellationRequestedAt = requestedAt;
            CancelledAt = requestedAt;
            Touch(requestedAt);
            return;
        }

        if (Status == AnalysisStatus.Running)
        {
            CancellationRequestedAt ??= requestedAt;
            Touch(requestedAt);
            return;
        }

        throw new DomainException("Only queued or running analyses can be cancelled.");
    }

    public void CancelFromWorker(DateTimeOffset cancelledAt)
    {
        EnsureStatus(AnalysisStatus.Running);
        if (CancellationRequestedAt is null)
        {
            throw new DomainException("Running analysis cancellation must be requested first.");
        }

        Status = AnalysisStatus.Cancelled;
        CancelledAt = cancelledAt;
        Touch(cancelledAt);
    }

    public void Complete(IEnumerable<string>? warnings, DateTimeOffset completedAt)
    {
        EnsureStatus(AnalysisStatus.Running);
        if (CancellationRequestedAt is not null)
        {
            throw new DomainException("An analysis with a cancellation request cannot complete.");
        }

        Status = AnalysisStatus.Completed;
        Warnings = warnings?
            .Where(warning => !string.IsNullOrWhiteSpace(warning))
            .Select(warning => RequireText(warning, nameof(warnings), 1_000))
            .ToArray() ?? [];
        CompletedAt = completedAt;
        Touch(completedAt);
    }

    public void RecordTransientFailure(
        string errorCode,
        string errorMessage,
        string correlationId,
        DateTimeOffset failedAt)
    {
        EnsureStatus(AnalysisStatus.Running);

        if (DeliveryAttemptCount < MaximumDeliveryAttempts)
        {
            Status = AnalysisStatus.Queued;
            SetError(errorCode, errorMessage, correlationId);
            Touch(failedAt);
            return;
        }

        Fail(errorCode, errorMessage, correlationId, failedAt);
    }

    public void Fail(
        string errorCode,
        string errorMessage,
        string correlationId,
        DateTimeOffset failedAt)
    {
        if (Status is not (AnalysisStatus.Queued or AnalysisStatus.Running))
        {
            throw new DomainException("Only queued or running analyses can fail.");
        }

        Status = AnalysisStatus.Failed;
        SetError(errorCode, errorMessage, correlationId);
        CompletedAt = failedAt;
        Touch(failedAt);
    }

    private void SetError(string errorCode, string errorMessage, string correlationId)
    {
        ErrorCode = RequireText(errorCode, nameof(errorCode), 100);
        ErrorMessage = RequireText(errorMessage, nameof(errorMessage), 500);
        CorrelationId = RequireText(correlationId, nameof(correlationId), 100);
    }

    private void EnsureStatus(AnalysisStatus expected)
    {
        if (Status != expected)
        {
            throw new DomainException($"Analysis must be {expected} but is {Status}.");
        }
    }
}

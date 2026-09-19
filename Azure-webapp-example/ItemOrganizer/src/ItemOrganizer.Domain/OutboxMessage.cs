namespace ItemOrganizer.Domain;

public sealed class OutboxMessage : OwnedEntity
{
    private OutboxMessage()
    {
    }

    public OutboxMessage(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid analysisId,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        AnalysisId = RequireId(analysisId, nameof(analysisId));
        Status = OutboxStatus.Pending;
        AvailableAt = createdAt;
    }

    public Guid AnalysisId { get; private set; }

    public OutboxStatus Status { get; private set; }

    public int AttemptCount { get; private set; }

    public DateTimeOffset AvailableAt { get; private set; }

    public DateTimeOffset? DispatchedAt { get; private set; }

    public string? LastErrorCode { get; private set; }

    public void MarkDispatched(DateTimeOffset dispatchedAt)
    {
        if (Status != OutboxStatus.Pending)
        {
            throw new DomainException("Only pending outbox messages can be dispatched.");
        }

        Status = OutboxStatus.Dispatched;
        DispatchedAt = dispatchedAt;
        AttemptCount++;
        Touch(dispatchedAt);
    }

    public void ScheduleRetry(string errorCode, DateTimeOffset availableAt)
    {
        if (Status != OutboxStatus.Pending)
        {
            throw new DomainException("Only pending outbox messages can be retried.");
        }

        AttemptCount++;
        LastErrorCode = RequireText(errorCode, nameof(errorCode), 100);
        AvailableAt = availableAt;
        Touch(availableAt);
    }

    public void MarkFailed(string errorCode, DateTimeOffset failedAt)
    {
        if (Status != OutboxStatus.Pending)
        {
            throw new DomainException("Only pending outbox messages can fail.");
        }

        Status = OutboxStatus.Failed;
        LastErrorCode = RequireText(errorCode, nameof(errorCode), 100);
        AttemptCount++;
        Touch(failedAt);
    }
}

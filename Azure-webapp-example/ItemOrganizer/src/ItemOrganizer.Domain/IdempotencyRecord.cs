namespace ItemOrganizer.Domain;

public sealed class IdempotencyRecord : OwnedEntity
{
    private IdempotencyRecord()
    {
    }

    public IdempotencyRecord(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        string operation,
        string key,
        string requestHash,
        DateTimeOffset expiresAt,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        Operation = RequireText(operation, nameof(operation), 100);
        Key = RequireText(key, nameof(key), 200);
        RequestHash = RequireText(requestHash, nameof(requestHash), 128);
        ExpiresAt = expiresAt > createdAt
            ? expiresAt
            : throw new DomainException("Idempotency expiration must follow creation.");
        Status = IdempotencyStatus.InProgress;
    }

    public string Operation { get; private set; } = string.Empty;

    public string Key { get; private set; } = string.Empty;

    public string RequestHash { get; private set; } = string.Empty;

    public IdempotencyStatus Status { get; private set; }

    public string? ResourceType { get; private set; }

    public Guid? ResourceId { get; private set; }

    public string? ErrorCode { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public void Complete(string resourceType, Guid resourceId, DateTimeOffset completedAt)
    {
        EnsureInProgress();
        Status = IdempotencyStatus.Completed;
        ResourceType = RequireText(resourceType, nameof(resourceType), 100);
        ResourceId = RequireId(resourceId, nameof(resourceId));
        Touch(completedAt);
    }

    public void Fail(string errorCode, DateTimeOffset failedAt)
    {
        EnsureInProgress();
        Status = IdempotencyStatus.Failed;
        ErrorCode = RequireText(errorCode, nameof(errorCode), 100);
        Touch(failedAt);
    }

    private void EnsureInProgress()
    {
        if (Status != IdempotencyStatus.InProgress)
        {
            throw new DomainException("Only in-progress idempotency records can change.");
        }
    }
}

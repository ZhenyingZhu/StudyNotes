namespace ItemOrganizer.Domain;

public abstract class Entity
{
    protected Entity()
    {
    }

    protected Entity(Guid id, DateTimeOffset createdAt)
    {
        Id = RequireId(id, nameof(id));
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        ConcurrencyToken = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; }

    public DateTimeOffset UpdatedAt { get; protected set; }

    public Guid ConcurrencyToken { get; private set; }

    internal void Touch(DateTimeOffset updatedAt)
    {
        if (updatedAt < CreatedAt)
        {
            throw new DomainException("Updated timestamp cannot precede the created timestamp.");
        }

        UpdatedAt = updatedAt;
    }

    internal void RefreshConcurrencyToken()
    {
        ConcurrencyToken = Guid.NewGuid();
    }

    protected static Guid RequireId(Guid value, string name)
    {
        return value == Guid.Empty
            ? throw new DomainException($"{name} must not be empty.")
            : value;
    }

    protected static string RequireText(string? value, string name, int maximumLength)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new DomainException($"{name} is required.");
        }

        if (normalized.Length > maximumLength)
        {
            throw new DomainException($"{name} must not exceed {maximumLength} characters.");
        }

        return normalized;
    }

    protected static string? OptionalText(string? value, string name, int maximumLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        if (normalized.Length > maximumLength)
        {
            throw new DomainException($"{name} must not exceed {maximumLength} characters.");
        }

        return normalized;
    }
}

public abstract class OwnedEntity : Entity
{
    protected OwnedEntity()
    {
    }

    protected OwnedEntity(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {
        TenantId = RequireId(tenantId, nameof(tenantId));
        OwnerObjectId = RequireId(ownerObjectId, nameof(ownerObjectId));
    }

    public Guid TenantId { get; protected set; }

    public Guid OwnerObjectId { get; protected set; }

    public bool IsOwnedBy(Guid tenantId, Guid ownerObjectId)
    {
        return TenantId == tenantId && OwnerObjectId == ownerObjectId;
    }
}

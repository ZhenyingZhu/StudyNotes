namespace ItemOrganizer.Domain;

public sealed class StorageContainer : OwnedEntity
{
    private StorageContainer()
    {
    }

    public StorageContainer(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        string name,
        string? description,
        string? location,
        IEnumerable<string>? labels,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        SetDetails(name, description, location, labels);
    }

    public string Name { get; private set; } = string.Empty;

    public string NormalizedName { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Location { get; private set; }

    public string[] Labels { get; private set; } = [];

    public DateTimeOffset? DeletedAt { get; private set; }

    public void Update(
        string name,
        string? description,
        string? location,
        IEnumerable<string>? labels,
        DateTimeOffset updatedAt)
    {
        EnsureNotDeleted();
        SetDetails(name, description, location, labels);
        Touch(updatedAt);
    }

    public void MarkDeleted(bool containsItems, DateTimeOffset deletedAt)
    {
        EnsureNotDeleted();
        if (containsItems)
        {
            throw new DomainException("A non-empty container cannot be deleted.");
        }

        DeletedAt = deletedAt;
        Touch(deletedAt);
    }

    private void SetDetails(
        string name,
        string? description,
        string? location,
        IEnumerable<string>? labels)
    {
        Name = RequireText(name, nameof(name), 200);
        NormalizedName = Name.ToUpperInvariant();
        Description = OptionalText(description, nameof(description), 2_000);
        Location = OptionalText(location, nameof(location), 500);
        Labels = labels?
            .Select(label => RequireText(label, nameof(labels), 100))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];
    }

    private void EnsureNotDeleted()
    {
        if (DeletedAt is not null)
        {
            throw new DomainException("Deleted containers cannot be changed.");
        }
    }
}

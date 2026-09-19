namespace ItemOrganizer.Domain;

public sealed class Item : OwnedEntity
{
    private Item()
    {
    }

    public Item(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid photoId,
        Guid analysisId,
        string name,
        string? description,
        string? category,
        int quantity,
        decimal confidence,
        string deduplicationKey,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        PhotoId = RequireId(photoId, nameof(photoId));
        AnalysisId = RequireId(analysisId, nameof(analysisId));
        DeduplicationKey = RequireText(deduplicationKey, nameof(deduplicationKey), 600);
        SetDetails(name, description, category, quantity);
        Confidence = confidence is >= 0 and <= 1
            ? confidence
            : throw new DomainException("Confidence must be between 0 and 1.");
    }

    public Guid PhotoId { get; private set; }

    public Guid AnalysisId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string NormalizedName { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Category { get; private set; }

    public string? NormalizedCategory { get; private set; }

    public int Quantity { get; private set; }

    public decimal Confidence { get; private set; }

    public string DeduplicationKey { get; private set; } = string.Empty;

    public DateTimeOffset? DeletedAt { get; private set; }

    public ItemAssignment Assignment { get; private set; } = null!;

    public void SetAssignment(ItemAssignment assignment)
    {
        if (assignment.ItemId != Id || !assignment.IsOwnedBy(TenantId, OwnerObjectId))
        {
            throw new DomainException("Item assignment ownership must match the item.");
        }

        Assignment = assignment;
    }

    public void Update(
        string name,
        string? description,
        string? category,
        int quantity,
        DateTimeOffset updatedAt)
    {
        EnsureNotDeleted();
        SetDetails(name, description, category, quantity);
        Touch(updatedAt);
    }

    public void MarkDeleted(DateTimeOffset deletedAt)
    {
        EnsureNotDeleted();
        DeletedAt = deletedAt;
        Touch(deletedAt);
    }

    private void SetDetails(string name, string? description, string? category, int quantity)
    {
        Name = RequireText(name, nameof(name), 300);
        NormalizedName = Name.ToUpperInvariant();
        Description = OptionalText(description, nameof(description), 2_000);
        Category = OptionalText(category, nameof(category), 200);
        NormalizedCategory = Category?.ToUpperInvariant();
        Quantity = quantity > 0
            ? quantity
            : throw new DomainException("Item quantity must be positive.");
    }

    private void EnsureNotDeleted()
    {
        if (DeletedAt is not null)
        {
            throw new DomainException("Deleted items cannot be changed.");
        }
    }
}

public sealed class ItemAssignment : OwnedEntity
{
    private ItemAssignment()
    {
    }

    private ItemAssignment(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid itemId,
        AssignmentStatus status,
        AssignmentSource source,
        Guid? containerId,
        Guid? suggestedContainerId,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        ItemId = RequireId(itemId, nameof(itemId));
        Status = status;
        Source = source;
        ContainerId = containerId;
        SuggestedContainerId = suggestedContainerId;
    }

    public Guid ItemId { get; private set; }

    public AssignmentStatus Status { get; private set; }

    public AssignmentSource Source { get; private set; }

    public Guid? ContainerId { get; private set; }

    public Guid? SuggestedContainerId { get; private set; }

    public static ItemAssignment Unassigned(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid itemId,
        DateTimeOffset createdAt)
    {
        return new(
            id,
            tenantId,
            ownerObjectId,
            itemId,
            AssignmentStatus.Unassigned,
            AssignmentSource.None,
            null,
            null,
            createdAt);
    }

    public static ItemAssignment Suggested(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid itemId,
        Guid suggestedContainerId,
        DateTimeOffset createdAt)
    {
        return new(
            id,
            tenantId,
            ownerObjectId,
            itemId,
            AssignmentStatus.Suggested,
            AssignmentSource.AiSuggestion,
            null,
            RequireId(suggestedContainerId, nameof(suggestedContainerId)),
            createdAt);
    }

    public static ItemAssignment Confirmed(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid itemId,
        Guid containerId,
        AssignmentSource source,
        DateTimeOffset createdAt)
    {
        if (source is not (AssignmentSource.User or AssignmentSource.ConvenienceWorkflow))
        {
            throw new DomainException("Confirmed assignments require a user-confirmed source.");
        }

        return new(
            id,
            tenantId,
            ownerObjectId,
            itemId,
            AssignmentStatus.Confirmed,
            source,
            RequireId(containerId, nameof(containerId)),
            null,
            createdAt);
    }

    public void AcceptSuggestion(Guid containerId, DateTimeOffset updatedAt)
    {
        if (Status != AssignmentStatus.Suggested || SuggestedContainerId != containerId)
        {
            throw new DomainException("Only the current suggested container can be accepted.");
        }

        Confirm(containerId, AssignmentSource.User, updatedAt);
    }

    public void Move(Guid containerId, DateTimeOffset updatedAt)
    {
        Confirm(RequireId(containerId, nameof(containerId)), AssignmentSource.User, updatedAt);
    }

    public void Remove(DateTimeOffset updatedAt)
    {
        Status = AssignmentStatus.Unassigned;
        Source = AssignmentSource.None;
        ContainerId = null;
        SuggestedContainerId = null;
        Touch(updatedAt);
    }

    private void Confirm(Guid containerId, AssignmentSource source, DateTimeOffset updatedAt)
    {
        Status = AssignmentStatus.Confirmed;
        Source = source;
        ContainerId = containerId;
        SuggestedContainerId = null;
        Touch(updatedAt);
    }
}

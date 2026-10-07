namespace ItemOrganizer.Domain;

public sealed class AnalysisDetection : OwnedEntity
{
    private AnalysisDetection()
    {
    }

    public AnalysisDetection(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        Guid analysisId,
        string name,
        string? description,
        string? category,
        int quantity,
        decimal confidence,
        Guid? suggestedContainerId,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        AnalysisId = RequireId(analysisId, nameof(analysisId));
        Name = RequireText(name, nameof(name), 300);
        Description = OptionalText(description, nameof(description), 2_000);
        Category = OptionalText(category, nameof(category), 200);
        Quantity = quantity > 0
            ? quantity
            : throw new DomainException("Detection quantity must be positive.");
        Confidence = confidence is >= 0 and <= 1
            ? confidence
            : throw new DomainException("Detection confidence must be between 0 and 1.");
        SuggestedContainerId = suggestedContainerId;
        ReviewStatus = DetectionReviewStatus.Pending;
    }

    public Guid AnalysisId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Category { get; private set; }

    public int Quantity { get; private set; }

    public decimal Confidence { get; private set; }

    public Guid? SuggestedContainerId { get; private set; }

    public DetectionReviewStatus ReviewStatus { get; private set; }

    public string? ReviewedName { get; private set; }

    public string? ReviewedDescription { get; private set; }

    public string? ReviewedCategory { get; private set; }

    public int? ReviewedQuantity { get; private set; }

    public Guid? SelectedContainerId { get; private set; }

    public Guid? ResultingItemId { get; private set; }

    public DateTimeOffset? ReviewedAt { get; private set; }

    public void Accept(
        string name,
        string? description,
        string? category,
        int quantity,
        Guid? selectedContainerId,
        Guid resultingItemId,
        DateTimeOffset reviewedAt)
    {
        EnsurePending();
        ReviewedName = RequireText(name, nameof(name), 300);
        ReviewedDescription = OptionalText(description, nameof(description), 2_000);
        ReviewedCategory = OptionalText(category, nameof(category), 200);
        ReviewedQuantity = quantity > 0
            ? quantity
            : throw new DomainException("Reviewed quantity must be positive.");
        SelectedContainerId = selectedContainerId;
        ResultingItemId = RequireId(resultingItemId, nameof(resultingItemId));
        ReviewStatus = DetectionReviewStatus.Accepted;
        ReviewedAt = reviewedAt;
        Touch(reviewedAt);
    }

    public void Reject(DateTimeOffset reviewedAt)
    {
        EnsurePending();
        ReviewStatus = DetectionReviewStatus.Rejected;
        ReviewedAt = reviewedAt;
        Touch(reviewedAt);
    }

    private void EnsurePending()
    {
        if (ReviewStatus != DetectionReviewStatus.Pending)
        {
            throw new DomainException("Only pending detections can be reviewed.");
        }
    }
}

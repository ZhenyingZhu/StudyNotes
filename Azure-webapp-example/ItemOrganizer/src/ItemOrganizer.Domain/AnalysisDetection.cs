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
        : this(
            id,
            tenantId,
            ownerObjectId,
            analysisId,
            name,
            description,
            category,
            quantity,
            confidence,
            suggestedContainerId,
            null,
            createdAt)
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
        NormalizedBoundingBox? predictedBoundingBox,
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
        SetPredictedBoundingBox(predictedBoundingBox);
        SetReviewedBoundingBox(predictedBoundingBox);
        ReviewStatus = DetectionReviewStatus.Pending;
    }

    public Guid AnalysisId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Category { get; private set; }

    public int Quantity { get; private set; }

    public decimal Confidence { get; private set; }

    public Guid? SuggestedContainerId { get; private set; }

    public decimal? PredictedBoundingBoxX { get; private set; }

    public decimal? PredictedBoundingBoxY { get; private set; }

    public decimal? PredictedBoundingBoxWidth { get; private set; }

    public decimal? PredictedBoundingBoxHeight { get; private set; }

    public decimal? ReviewedBoundingBoxX { get; private set; }

    public decimal? ReviewedBoundingBoxY { get; private set; }

    public decimal? ReviewedBoundingBoxWidth { get; private set; }

    public decimal? ReviewedBoundingBoxHeight { get; private set; }

    public DetectionReviewStatus ReviewStatus { get; private set; }

    public string? ReviewedName { get; private set; }

    public string? ReviewedDescription { get; private set; }

    public string? ReviewedCategory { get; private set; }

    public int? ReviewedQuantity { get; private set; }

    public Guid? SelectedContainerId { get; private set; }

    public Guid? ResultingItemId { get; private set; }

    public DateTimeOffset? ReviewedAt { get; private set; }

    public NormalizedBoundingBox? GetPredictedBoundingBox() =>
        CreateBoundingBox(
            PredictedBoundingBoxX,
            PredictedBoundingBoxY,
            PredictedBoundingBoxWidth,
            PredictedBoundingBoxHeight);

    public NormalizedBoundingBox? GetReviewedBoundingBox() =>
        CreateBoundingBox(
            ReviewedBoundingBoxX,
            ReviewedBoundingBoxY,
            ReviewedBoundingBoxWidth,
            ReviewedBoundingBoxHeight);

    public void Accept(
        string name,
        string? description,
        string? category,
        int quantity,
        Guid? selectedContainerId,
        Guid resultingItemId,
        DateTimeOffset reviewedAt)
    {
        Accept(
            name,
            description,
            category,
            quantity,
            selectedContainerId,
            GetReviewedBoundingBox(),
            resultingItemId,
            reviewedAt);
    }

    public void Accept(
        string name,
        string? description,
        string? category,
        int quantity,
        Guid? selectedContainerId,
        NormalizedBoundingBox? reviewedBoundingBox,
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
        SetReviewedBoundingBox(reviewedBoundingBox);
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

    private void SetPredictedBoundingBox(NormalizedBoundingBox? boundingBox)
    {
        PredictedBoundingBoxX = boundingBox?.X;
        PredictedBoundingBoxY = boundingBox?.Y;
        PredictedBoundingBoxWidth = boundingBox?.Width;
        PredictedBoundingBoxHeight = boundingBox?.Height;
    }

    private void SetReviewedBoundingBox(NormalizedBoundingBox? boundingBox)
    {
        ReviewedBoundingBoxX = boundingBox?.X;
        ReviewedBoundingBoxY = boundingBox?.Y;
        ReviewedBoundingBoxWidth = boundingBox?.Width;
        ReviewedBoundingBoxHeight = boundingBox?.Height;
    }

    private static NormalizedBoundingBox? CreateBoundingBox(
        decimal? x,
        decimal? y,
        decimal? width,
        decimal? height)
    {
        if (x is null && y is null && width is null && height is null)
        {
            return null;
        }

        if (x is null || y is null || width is null || height is null)
        {
            throw new DomainException("Bounding box coordinates are incomplete.");
        }

        return new(x.Value, y.Value, width.Value, height.Value);
    }
}

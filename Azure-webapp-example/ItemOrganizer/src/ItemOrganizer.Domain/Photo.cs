namespace ItemOrganizer.Domain;

public sealed class Photo : OwnedEntity
{
    private Photo()
    {
    }

    public Photo(
        Guid id,
        Guid tenantId,
        Guid ownerObjectId,
        string blobName,
        string contentType,
        long contentLength,
        int width,
        int height,
        string sha256,
        DateTimeOffset retainUntil,
        DateTimeOffset createdAt)
        : base(id, tenantId, ownerObjectId, createdAt)
    {
        BlobName = RequireText(blobName, nameof(blobName), 1_024);
        ContentType = RequireContentType(contentType);
        ContentLength = contentLength is > 0 and <= 10 * 1024 * 1024
            ? contentLength
            : throw new DomainException("Photo size must be between 1 byte and 10 MiB.");
        Width = width is >= 512 and <= 8_000
            ? width
            : throw new DomainException("Photo width must be between 512 and 8000 pixels.");
        Height = height is >= 512 and <= 8_000
            ? height
            : throw new DomainException("Photo height must be between 512 and 8000 pixels.");
        Sha256 = RequireSha256(sha256);
        RetainUntil = retainUntil >= createdAt
            ? retainUntil
            : throw new DomainException("Photo retention cannot end before creation.");
        RetentionState = PhotoRetentionState.Active;
    }

    public string BlobName { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long ContentLength { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public string Sha256 { get; private set; } = string.Empty;

    public DateTimeOffset RetainUntil { get; private set; }

    public PhotoRetentionState RetentionState { get; private set; }

    public DateTimeOffset? DeletionRequestedAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public void RequestDeletion(bool hasRunningAnalysis, DateTimeOffset requestedAt)
    {
        if (RetentionState == PhotoRetentionState.Deleted)
        {
            return;
        }

        if (hasRunningAnalysis)
        {
            throw new DomainException("A photo cannot be deleted while analysis is running.");
        }

        RetentionState = PhotoRetentionState.PendingDeletion;
        DeletionRequestedAt ??= requestedAt;
        Touch(requestedAt);
    }

    public void MarkDeleted(DateTimeOffset deletedAt)
    {
        if (RetentionState != PhotoRetentionState.PendingDeletion)
        {
            throw new DomainException("Photo deletion must be requested before completion.");
        }

        RetentionState = PhotoRetentionState.Deleted;
        DeletedAt = deletedAt;
        Touch(deletedAt);
    }

    private static string RequireContentType(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "image/jpeg" => "image/jpeg",
            "image/png" => "image/png",
            "image/webp" => "image/webp",
            _ => throw new DomainException("Unsupported photo content type.")
        };
    }

    private static string RequireSha256(string value)
    {
        var normalized = value.Trim().ToLowerInvariant();
        if (normalized.Length != 64 || normalized.Any(character => !Uri.IsHexDigit(character)))
        {
            throw new DomainException("SHA-256 must be a 64-character hexadecimal value.");
        }

        return normalized;
    }
}

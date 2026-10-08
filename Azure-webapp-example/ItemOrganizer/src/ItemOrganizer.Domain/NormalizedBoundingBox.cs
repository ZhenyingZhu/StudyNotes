namespace ItemOrganizer.Domain;

public sealed record NormalizedBoundingBox
{
    public NormalizedBoundingBox(
        decimal x,
        decimal y,
        decimal width,
        decimal height)
    {
        if (x < 0
            || y < 0
            || width <= 0
            || height <= 0
            || x > 1
            || y > 1
            || width > 1
            || height > 1
            || x + width > 1
            || y + height > 1)
        {
            throw new DomainException(
                "Bounding boxes must have positive dimensions and remain within the photo.");
        }

        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public decimal X { get; }

    public decimal Y { get; }

    public decimal Width { get; }

    public decimal Height { get; }
}

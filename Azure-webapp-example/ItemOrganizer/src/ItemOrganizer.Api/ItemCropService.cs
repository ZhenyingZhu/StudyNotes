using System.Security.Cryptography;
using Azure;
using ItemOrganizer.Domain;
using SkiaSharp;

namespace ItemOrganizer.Api;

public sealed record ItemCropRequest(
    Guid ItemId,
    NormalizedBoundingBox BoundingBox);

public sealed record ItemCropResult(
    Guid ItemId,
    string BlobName,
    NormalizedBoundingBox BoundingBox,
    int PixelWidth,
    int PixelHeight,
    string ContentType,
    long ContentLength,
    string Sha256);

public sealed class ItemCropService(
    IPhotoStorage storage,
    ILogger<ItemCropService> logger)
{
    public async Task<IReadOnlyList<ItemCropResult>> CreateAsync(
        Photo photo,
        IReadOnlyList<ItemCropRequest> requests,
        CancellationToken cancellationToken)
    {
        if (requests.Count == 0)
        {
            return [];
        }

        var sourceBytes = await storage.DownloadAsync(
            photo.BlobName,
            cancellationToken);
        using var source = SKBitmap.Decode(sourceBytes)
            ?? throw new InvalidOperationException(
                "The stored source photo could not be decoded.");
        var results = new List<ItemCropResult>(requests.Count);
        try
        {
            foreach (var request in requests)
            {
                var sourceRect = ToPixelRect(
                    request.BoundingBox,
                    source.Width,
                    source.Height);
                using var crop = new SKBitmap(
                    sourceRect.Width,
                    sourceRect.Height,
                    source.ColorType,
                    source.AlphaType);
                using (var canvas = new SKCanvas(crop))
                {
                    canvas.DrawBitmap(
                        source,
                        sourceRect,
                        new SKRect(
                            0,
                            0,
                            sourceRect.Width,
                            sourceRect.Height),
                        new SKSamplingOptions(
                            SKFilterMode.Linear,
                            SKMipmapMode.None));
                }

                using var image = SKImage.FromBitmap(crop);
                using var encoded = image.Encode(
                    SKEncodedImageFormat.Png,
                    100);
                var bytes = encoded.ToArray();
                var blobName =
                    $"{photo.TenantId:N}/{photo.OwnerObjectId:N}/item-crops/{request.ItemId:N}.png";
                using var stream = new MemoryStream(bytes, writable: false);
                await storage.UploadAsync(
                    blobName,
                    stream,
                    "image/png",
                    cancellationToken);
                results.Add(new(
                    request.ItemId,
                    blobName,
                    request.BoundingBox,
                    sourceRect.Width,
                    sourceRect.Height,
                    "image/png",
                    bytes.LongLength,
                    Convert.ToHexString(SHA256.HashData(bytes))
                        .ToLowerInvariant()));
            }

            return results;
        }
        catch
        {
            await DeleteCreatedAsync(results, cancellationToken);
            throw;
        }
    }

    public async Task DeleteCreatedAsync(
        IEnumerable<ItemCropResult> crops,
        CancellationToken cancellationToken)
    {
        foreach (var crop in crops)
        {
            try
            {
                await storage.DeleteIfExistsAsync(
                    crop.BlobName,
                    cancellationToken);
            }
            catch (RequestFailedException exception)
            {
                logger.LogError(
                    exception,
                    "Compensating deletion failed for item crop {ItemId}.",
                    crop.ItemId);
            }
        }
    }

    private static SKRectI ToPixelRect(
        NormalizedBoundingBox box,
        int width,
        int height)
    {
        var left = Math.Clamp(
            (int)Math.Floor((double)box.X * width),
            0,
            width - 1);
        var top = Math.Clamp(
            (int)Math.Floor((double)box.Y * height),
            0,
            height - 1);
        var right = Math.Clamp(
            (int)Math.Ceiling((double)(box.X + box.Width) * width),
            left + 1,
            width);
        var bottom = Math.Clamp(
            (int)Math.Ceiling((double)(box.Y + box.Height) * height),
            top + 1,
            height);
        return new(left, top, right, bottom);
    }
}

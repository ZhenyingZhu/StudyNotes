using Azure;
using ItemOrganizer.Api;

namespace ItemOrganizer.Api.Tests;

public sealed class FakePhotoStorage : IPhotoStorage
{
    private readonly Dictionary<string, StoredPhoto> _photos = [];

    public bool FailUploads { get; set; }

    public bool FailDeletes { get; set; }

    public IReadOnlyDictionary<string, StoredPhoto> Photos => _photos;

    public void Seed(
        string blobName,
        byte[] content,
        string contentType)
    {
        _photos[blobName] = new StoredPhoto(content, contentType);
    }

    public Task UploadAsync(
        string blobName,
        Stream content,
        string contentType,
        CancellationToken cancellationToken)
    {
        if (FailUploads)
        {
            throw new RequestFailedException(503, "Storage unavailable.");
        }

        using var buffer = new MemoryStream();
        content.CopyTo(buffer);
        _photos.Add(
            blobName,
            new StoredPhoto(buffer.ToArray(), contentType));
        return Task.CompletedTask;
    }

    public Task<Uri> CreateReadUriAsync(
        string blobName,
        TimeSpan lifetime,
        CancellationToken cancellationToken)
    {
        if (!_photos.ContainsKey(blobName))
        {
            throw new FileNotFoundException();
        }

        return Task.FromResult(
            new Uri(
                $"https://storage.example.invalid/photos/{Uri.EscapeDataString(blobName)}?expires={lifetime.TotalMinutes:0}"));
    }

    public Task<byte[]> DownloadAsync(
        string blobName,
        CancellationToken cancellationToken)
    {
        if (!_photos.TryGetValue(blobName, out var photo))
        {
            throw new FileNotFoundException();
        }

        return Task.FromResult(photo.Content.ToArray());
    }

    public Task DeleteIfExistsAsync(
        string blobName,
        CancellationToken cancellationToken)
    {
        if (FailDeletes)
        {
            throw new RequestFailedException(503, "Storage unavailable.");
        }

        _photos.Remove(blobName);
        return Task.CompletedTask;
    }

    public sealed record StoredPhoto(byte[] Content, string ContentType);
}

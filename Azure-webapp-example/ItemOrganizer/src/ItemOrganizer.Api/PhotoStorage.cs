using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace ItemOrganizer.Api;

public interface IPhotoStorage
{
    Task UploadAsync(
        string blobName,
        Stream content,
        string contentType,
        CancellationToken cancellationToken);

    Task<Uri> CreateReadUriAsync(
        string blobName,
        TimeSpan lifetime,
        CancellationToken cancellationToken);

    Task<byte[]> DownloadAsync(
        string blobName,
        CancellationToken cancellationToken);

    Task DeleteIfExistsAsync(
        string blobName,
        CancellationToken cancellationToken);
}

public sealed class AzureBlobPhotoStorage(
    BlobServiceClient serviceClient,
    IConfiguration configuration) : IPhotoStorage
{
    private readonly BlobContainerClient _container =
        serviceClient.GetBlobContainerClient(
            configuration["PhotoStorage:ContainerName"] ?? "photos");

    public async Task UploadAsync(
        string blobName,
        Stream content,
        string contentType,
        CancellationToken cancellationToken)
    {
        await EnsureContainerAsync(cancellationToken);
        await _container.GetBlobClient(blobName).UploadAsync(
            content,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType,
                    CacheControl = "private, no-store"
                }
            },
            cancellationToken);
    }

    public async Task<Uri> CreateReadUriAsync(
        string blobName,
        TimeSpan lifetime,
        CancellationToken cancellationToken)
    {
        await EnsureContainerAsync(cancellationToken);
        var blob = _container.GetBlobClient(blobName);
        if (!await blob.ExistsAsync(cancellationToken))
        {
            throw new FileNotFoundException("The photo content does not exist.");
        }
        if (!blob.CanGenerateSasUri)
        {
            throw new InvalidOperationException(
                "The configured storage credential cannot issue a read URL.");
        }

        var now = DateTimeOffset.UtcNow;
        var sas = new BlobSasBuilder(
            BlobSasPermissions.Read,
            now.Add(lifetime))
        {
            BlobContainerName = _container.Name,
            BlobName = blobName,
            Resource = "b",
            StartsOn = now.AddMinutes(-1)
        };
        return blob.GenerateSasUri(sas);
    }

    public async Task<byte[]> DownloadAsync(
        string blobName,
        CancellationToken cancellationToken)
    {
        await EnsureContainerAsync(cancellationToken);
        var response = await _container
            .GetBlobClient(blobName)
            .DownloadContentAsync(cancellationToken);
        return response.Value.Content.ToArray();
    }

    public async Task DeleteIfExistsAsync(
        string blobName,
        CancellationToken cancellationToken)
    {
        await EnsureContainerAsync(cancellationToken);
        await _container.GetBlobClient(blobName).DeleteIfExistsAsync(
            DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);
    }

    private Task EnsureContainerAsync(CancellationToken cancellationToken)
    {
        return _container.CreateIfNotExistsAsync(
            PublicAccessType.None,
            cancellationToken: cancellationToken);
    }
}

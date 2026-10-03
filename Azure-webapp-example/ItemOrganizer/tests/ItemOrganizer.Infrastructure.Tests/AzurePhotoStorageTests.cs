using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ItemOrganizer.Api;
using Microsoft.Extensions.Configuration;

namespace ItemOrganizer.Infrastructure.Tests;

public sealed class AzurePhotoStorageTests
{
    [Fact]
    public async Task Private_blob_can_be_uploaded_read_and_deleted()
    {
        var connectionString =
            Environment.GetEnvironmentVariable(
                "ITEMORGANIZER_STORAGE_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ITEMORGANIZER_STORAGE_CONNECTION is required for storage integration tests.");
        }

        var containerName = $"photos-{Guid.NewGuid():N}";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PhotoStorage:ContainerName"] = containerName
            })
            .Build();
        var serviceClient = new BlobServiceClient(
            connectionString,
            new BlobClientOptions(
                BlobClientOptions.ServiceVersion.V2024_11_04));
        var storage = new AzureBlobPhotoStorage(
            serviceClient,
            configuration);
        const string blobName = "photos/fixture";
        var content = new byte[] { 1, 2, 3, 4 };

        try
        {
            await storage.UploadAsync(
                blobName,
                new MemoryStream(content),
                "image/png",
                CancellationToken.None);

            var container =
                serviceClient.GetBlobContainerClient(containerName);
            var properties = await container.GetPropertiesAsync();
            Assert.Equal(
                PublicAccessType.None,
                properties.Value.PublicAccess);

            var readUri = await storage.CreateReadUriAsync(
                blobName,
                TimeSpan.FromMinutes(5),
                CancellationToken.None);
            using var httpClient = new HttpClient();
            Assert.Equal(
                content,
                await httpClient.GetByteArrayAsync(readUri));
            Assert.Equal(
                content,
                await storage.DownloadAsync(
                    blobName,
                    CancellationToken.None));

            await storage.DeleteIfExistsAsync(
                blobName,
                CancellationToken.None);
            await storage.DeleteIfExistsAsync(
                blobName,
                CancellationToken.None);
            Assert.False(
                await container.GetBlobClient(blobName).ExistsAsync());
        }
        finally
        {
            await serviceClient
                .GetBlobContainerClient(containerName)
                .DeleteIfExistsAsync();
        }
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ItemOrganizer.Domain;
using ItemOrganizer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace ItemOrganizer.Api.Tests;

public sealed class PhotoApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Valid_upload_is_private_and_idempotent()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var key = Guid.NewGuid().ToString("N");
        var image = CreatePng(512, 512);
        var existingBlobNames =
            factory.PhotoStorage.Photos.Keys.ToHashSet();

        var first = await UploadAsync(client, image, "image/png", key);

        Assert.True(
            first.StatusCode == HttpStatusCode.Created,
            await first.Content.ReadAsStringAsync());
        Assert.NotNull(first.Headers.Location);
        Assert.NotNull(first.Headers.ETag);
        var firstDocument =
            await first.Content.ReadFromJsonAsync<JsonDocument>();
        var photoId = firstDocument!.RootElement.GetProperty("id").GetGuid();
        Assert.Equal(
            512,
            firstDocument.RootElement.GetProperty("width").GetInt32());
        var stored = Assert.Single(
            factory.PhotoStorage.Photos,
            entry => !existingBlobNames.Contains(entry.Key));
        Assert.DoesNotContain(
            ApiFactory.TenantId.ToString(),
            stored.Key,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("photo.png", stored.Key);

        var replay = await UploadAsync(client, image, "image/png", key);

        Assert.Equal(HttpStatusCode.OK, replay.StatusCode);
        var replayDocument =
            await replay.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            photoId,
            replayDocument!.RootElement.GetProperty("id").GetGuid());
        Assert.Equal(
            existingBlobNames.Count + 1,
            factory.PhotoStorage.Photos.Count);
    }

    [Fact]
    public async Task Reusing_idempotency_key_for_different_photo_is_conflict()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var key = Guid.NewGuid().ToString("N");
        var first = await UploadAsync(
            client,
            CreatePng(512, 512, SKColors.Blue),
            "image/png",
            key);
        first.EnsureSuccessStatusCode();

        var response = await UploadAsync(
            client,
            CreatePng(512, 512, SKColors.Red),
            "image/png",
            key);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Invalid_uploads_do_not_create_blobs()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var before = factory.PhotoStorage.Photos.Count;

        var unsupported = await UploadAsync(
            client,
            Encoding.UTF8.GetBytes("not an image"),
            "text/plain");
        var tooSmall = await UploadAsync(
            client,
            CreatePng(128, 128),
            "image/png");
        var mismatched = await UploadAsync(
            client,
            CreatePng(512, 512),
            "image/jpeg");

        Assert.Equal(
            HttpStatusCode.UnsupportedMediaType,
            unsupported.StatusCode);
        Assert.Equal(
            HttpStatusCode.UnprocessableEntity,
            tooSmall.StatusCode);
        Assert.Equal(
            HttpStatusCode.UnsupportedMediaType,
            mismatched.StatusCode);
        Assert.Equal(before, factory.PhotoStorage.Photos.Count);
    }

    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    [InlineData("image/webp")]
    public async Task Accepted_static_formats_can_be_uploaded(
        string contentType)
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");

        var response = await UploadAsync(
            client,
            CreateImage(512, 512, contentType),
            contentType);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Oversized_upload_and_quota_limit_are_rejected()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var oversized = await UploadAsync(
            client,
            new byte[(10 * 1024 * 1024) + 1],
            "image/png");
        Assert.Equal(
            HttpStatusCode.RequestEntityTooLarge,
            oversized.StatusCode);

        var configuration =
            factory.Services.GetRequiredService<IConfiguration>();
        var previous = configuration["PhotoStorage:MaximumPhotosPerOwner"];
        configuration["PhotoStorage:MaximumPhotosPerOwner"] = "1";
        try
        {
            var quota = await UploadAsync(
                client,
                CreatePng(512, 512),
                "image/png");

            Assert.Equal(
                HttpStatusCode.TooManyRequests,
                quota.StatusCode);
            Assert.Equal(
                TimeSpan.FromHours(1),
                quota.Headers.RetryAfter?.Delta);
        }
        finally
        {
            configuration["PhotoStorage:MaximumPhotosPerOwner"] = previous;
        }
    }

    [Fact]
    public async Task Authorized_content_request_returns_short_lived_url()
    {
        using var writeClient =
            CreateAuthenticatedClient("ItemOrganizer.Write");
        var uploaded = await UploadAsync(
            writeClient,
            CreatePng(512, 512),
            "image/png");
        var document =
            await uploaded.Content.ReadFromJsonAsync<JsonDocument>();
        var photoId = document!.RootElement.GetProperty("id").GetGuid();

        using var anonymousClient = factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        var unauthorized = await anonymousClient.GetAsync(
            $"/api/v1/photos/{photoId}/content");

        using var readClient = CreateAuthenticatedClient(
            "ItemOrganizer.Read",
            allowAutoRedirect: false);
        var authorized = await readClient.GetAsync(
            $"/api/v1/photos/{photoId}/content");

        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, authorized.StatusCode);
        Assert.Equal(
            "storage.example.invalid",
            authorized.Headers.Location?.Host);
        Assert.True(authorized.Headers.CacheControl?.Private);
        Assert.True(authorized.Headers.CacheControl?.NoStore);
    }

    [Fact]
    public async Task Another_owner_cannot_request_photo_content()
    {
        using var ownerClient =
            CreateAuthenticatedClient("ItemOrganizer.Write");
        var uploaded = await UploadAsync(
            ownerClient,
            CreatePng(512, 512),
            "image/png");
        var document =
            await uploaded.Content.ReadFromJsonAsync<JsonDocument>();
        var photoId = document!.RootElement.GetProperty("id").GetGuid();

        using var otherOwnerClient = CreateAuthenticatedClient(
            "ItemOrganizer.Read",
            ownerId: ApiFactory.OtherOwnerId,
            allowAutoRedirect: false);
        var response = await otherOwnerClient.GetAsync(
            $"/api/v1/photos/{photoId}/content");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_is_blocked_during_analysis_and_is_idempotent()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write");
        var uploaded = await UploadAsync(
            client,
            CreatePng(512, 512),
            "image/png");
        var document =
            await uploaded.Content.ReadFromJsonAsync<JsonDocument>();
        var photoId = document!.RootElement.GetProperty("id").GetGuid();

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>();
            dbContext.Analyses.Add(new Analysis(
                Guid.NewGuid(),
                ApiFactory.TenantId,
                ApiFactory.OwnerId,
                photoId,
                "test",
                "test",
                "mock",
                "tests",
                null,
                DateTimeOffset.UtcNow));
            await dbContext.SaveChangesAsync();
        }

        var blocked = await client.DeleteAsync($"/api/v1/photos/{photoId}");
        Assert.Equal(HttpStatusCode.Conflict, blocked.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>();
            var analysis = await dbContext.Analyses.SingleAsync(
                entity => entity.PhotoId == photoId);
            analysis.RequestCancellation(DateTimeOffset.UtcNow);
            await dbContext.SaveChangesAsync();
        }

        var deleted = await client.DeleteAsync($"/api/v1/photos/{photoId}");
        var retried = await client.DeleteAsync($"/api/v1/photos/{photoId}");

        Assert.Equal(HttpStatusCode.NoContent, deleted.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, retried.StatusCode);
    }

    [Fact]
    public async Task Storage_failure_returns_safe_problem_without_metadata()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        int before;
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>();
            before = await dbContext.Photos.CountAsync();
        }
        factory.PhotoStorage.FailUploads = true;
        try
        {
            var response = await UploadAsync(
                client,
                CreatePng(512, 512),
                "image/png");

            Assert.Equal(
                HttpStatusCode.ServiceUnavailable,
                response.StatusCode);
            var problem =
                await response.Content.ReadFromJsonAsync<JsonDocument>();
            Assert.Equal(
                "Photo storage is temporarily unavailable.",
                problem!.RootElement.GetProperty("detail").GetString());
            using var scope = factory.Services.CreateScope();
            var dbContext =
                scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>();
            Assert.Equal(before, await dbContext.Photos.CountAsync());
        }
        finally
        {
            factory.PhotoStorage.FailUploads = false;
        }
    }

    [Fact]
    public async Task Delete_storage_failure_remains_pending_and_can_retry()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var uploaded = await UploadAsync(
            client,
            CreatePng(512, 512),
            "image/png");
        var document =
            await uploaded.Content.ReadFromJsonAsync<JsonDocument>();
        var photoId = document!.RootElement.GetProperty("id").GetGuid();

        factory.PhotoStorage.FailDeletes = true;
        var failed = await client.DeleteAsync($"/api/v1/photos/{photoId}");
        factory.PhotoStorage.FailDeletes = false;

        Assert.Equal(HttpStatusCode.ServiceUnavailable, failed.StatusCode);
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext =
                scope.ServiceProvider.GetRequiredService<ItemOrganizerDbContext>();
            var photo = await dbContext.Photos.SingleAsync(
                entity => entity.Id == photoId);
            Assert.Equal(
                PhotoRetentionState.PendingDeletion,
                photo.RetentionState);
            Assert.Null(photo.DeletedAt);
        }

        var retried = await client.DeleteAsync($"/api/v1/photos/{photoId}");
        Assert.Equal(HttpStatusCode.NoContent, retried.StatusCode);
    }

    private static async Task<HttpResponseMessage> UploadAsync(
        HttpClient client,
        byte[] content,
        string contentType,
        string? idempotencyKey = null)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/v1/photos");
        using var multipart = new MultipartFormDataContent();
        var file = new ByteArrayContent(content);
        file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        multipart.Add(file, "file", "photo.png");
        request.Content = multipart;
        if (idempotencyKey is not null)
        {
            request.Headers.Add("Idempotency-Key", idempotencyKey);
        }
        return await client.SendAsync(request);
    }

    private static byte[] CreatePng(
        int width,
        int height,
        SKColor? color = null)
    {
        return CreateImage(
            width,
            height,
            "image/png",
            color);
    }

    private static byte[] CreateImage(
        int width,
        int height,
        string contentType,
        SKColor? color = null)
    {
        using var bitmap = new SKBitmap(width, height);
        bitmap.Erase(color ?? SKColors.Green);
        using var image = SKImage.FromBitmap(bitmap);
        var format = contentType switch
        {
            "image/jpeg" => SKEncodedImageFormat.Jpeg,
            "image/png" => SKEncodedImageFormat.Png,
            "image/webp" => SKEncodedImageFormat.Webp,
            _ => throw new ArgumentOutOfRangeException(nameof(contentType))
        };
        using var data = image.Encode(format, 90);
        return data.ToArray();
    }

    private HttpClient CreateAuthenticatedClient(
        string scope,
        Guid? ownerId = null,
        bool allowAutoRedirect = true)
    {
        var client = factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect
            });
        client.DefaultRequestHeaders.Add(
            "X-Test-Tenant",
            ApiFactory.TenantId.ToString());
        client.DefaultRequestHeaders.Add(
            "X-Test-Owner",
            (ownerId ?? ApiFactory.OwnerId).ToString());
        client.DefaultRequestHeaders.Add("X-Test-Scope", scope);
        return client;
    }
}

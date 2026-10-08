using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ItemOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;

namespace ItemOrganizer.Api.Tests;

public sealed class AnalysisApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Start_analysis_returns_queued_resource_and_outbox_entry()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
        var photoId = await UploadPhotoAsync(client);

        var response = await client.PostAsync(
            $"/api/v1/photos/{photoId}/analyses",
            null);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.NotNull(response.Headers.ETag);
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            "queued",
            document!.RootElement.GetProperty("status").GetString());

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<
            ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
        var analysisId = document.RootElement.GetProperty("id").GetGuid();
        Assert.True(await dbContext.OutboxMessages.AnyAsync(
            message => message.AnalysisId == analysisId));
    }

    [Fact]
    public async Task Start_analysis_is_owner_scoped_and_idempotent()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
        var photoId = await UploadPhotoAsync(client);
        var key = Guid.NewGuid().ToString("N");

        using var firstRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/photos/{photoId}/analyses");
        firstRequest.Headers.Add("Idempotency-Key", key);
        var first = await client.SendAsync(firstRequest);
        var firstDocument =
            await first.Content.ReadFromJsonAsync<JsonDocument>();

        using var replayRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/photos/{photoId}/analyses");
        replayRequest.Headers.Add("Idempotency-Key", key);
        var replay = await client.SendAsync(replayRequest);
        var replayDocument =
            await replay.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.Equal(HttpStatusCode.Accepted, first.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, replay.StatusCode);
        Assert.Equal(
            firstDocument!.RootElement.GetProperty("id").GetGuid(),
            replayDocument!.RootElement.GetProperty("id").GetGuid());

        using var otherOwner = CreateAuthenticatedClient(
            "ItemOrganizer.Analyze",
            ApiFactory.OtherOwnerId);
        var hidden = await otherOwner.PostAsync(
            $"/api/v1/photos/{photoId}/analyses",
            null);
        Assert.Equal(HttpStatusCode.NotFound, hidden.StatusCode);
    }

    [Fact]
    public async Task Analyze_scope_is_required()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");
        var photoId = await UploadPhotoAsync(client);

        var response = await client.PostAsync(
            $"/api/v1/photos/{photoId}/analyses",
            null);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Convenience_workflow_uploads_photo_and_targets_container()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
        using var content = CreatePhotoContent();

        var response = await client.PostAsync(
            $"/api/v1/containers/{ApiFactory.ContainerId}/photo-analyses",
            content);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.NotNull(response.Headers.ETag);
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var analysisId = document!.RootElement.GetProperty("id").GetGuid();
        var photoId = document.RootElement.GetProperty("photoId").GetGuid();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<
            ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
        var analysis = await dbContext.Analyses.SingleAsync(
            entity => entity.Id == analysisId);
        Assert.Equal(ApiFactory.ContainerId, analysis.ConfirmedContainerId);
        Assert.True(await dbContext.Photos.AnyAsync(
            photo => photo.Id == photoId));
        Assert.True(await dbContext.OutboxMessages.AnyAsync(
            message => message.AnalysisId == analysisId));
    }

    [Fact]
    public async Task Convenience_workflow_validates_container_before_upload()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Write ItemOrganizer.Analyze");
        var before = factory.PhotoStorage.Photos.Count;
        using var content = CreatePhotoContent();

        var response = await client.PostAsync(
            $"/api/v1/containers/{Guid.NewGuid()}/photo-analyses",
            content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(before, factory.PhotoStorage.Photos.Count);
    }

    [Fact]
    public async Task Convenience_workflow_is_idempotent_as_one_operation()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Write ItemOrganizer.Analyze");
        var key = Guid.NewGuid().ToString("N");

        using var firstRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/containers/{ApiFactory.ContainerId}/photo-analyses")
        {
            Content = CreatePhotoContent()
        };
        firstRequest.Headers.Add("Idempotency-Key", key);
        var first = await client.SendAsync(firstRequest);
        var firstDocument =
            await first.Content.ReadFromJsonAsync<JsonDocument>();

        using var replayRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/containers/{ApiFactory.ContainerId}/photo-analyses")
        {
            Content = CreatePhotoContent()
        };
        replayRequest.Headers.Add("Idempotency-Key", key);
        var replay = await client.SendAsync(replayRequest);
        var replayDocument =
            await replay.Content.ReadFromJsonAsync<JsonDocument>();

        Assert.Equal(HttpStatusCode.Accepted, first.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, replay.StatusCode);
        Assert.Equal(
            firstDocument!.RootElement.GetProperty("id").GetGuid(),
            replayDocument!.RootElement.GetProperty("id").GetGuid());
        Assert.Equal(
            firstDocument.RootElement.GetProperty("photoId").GetGuid(),
            replayDocument.RootElement.GetProperty("photoId").GetGuid());
    }

    [Fact]
    public async Task Convenience_storage_failure_creates_no_analysis()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Write ItemOrganizer.Analyze");
        int before;
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<
                ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
            before = await dbContext.Analyses.CountAsync();
        }

        factory.PhotoStorage.FailUploads = true;
        try
        {
            using var content = CreatePhotoContent();
            var response = await client.PostAsync(
                $"/api/v1/containers/{ApiFactory.ContainerId}/photo-analyses",
                content);

            Assert.Equal(
                HttpStatusCode.ServiceUnavailable,
                response.StatusCode);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<
                ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
            Assert.Equal(before, await dbContext.Analyses.CountAsync());
        }
        finally
        {
            factory.PhotoStorage.FailUploads = false;
        }
    }

    [Fact]
    public async Task Queued_analysis_can_be_cancelled_with_current_etag()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
        var photoId = await UploadPhotoAsync(client);
        var started = await client.PostAsync(
            $"/api/v1/photos/{photoId}/analyses",
            null);
        var document =
            await started.Content.ReadFromJsonAsync<JsonDocument>();
        var analysisId = document!.RootElement.GetProperty("id").GetGuid();

        using var cancel = new HttpRequestMessage(
            HttpMethod.Post,
            $"/api/v1/analyses/{analysisId}/cancel");
        cancel.Headers.IfMatch.Add(
            EntityTagHeaderValue.Parse(started.Headers.ETag!.ToString()));
        var response = await client.SendAsync(cancel);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var cancelled =
            await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            "cancelled",
            cancelled!.RootElement.GetProperty("status").GetString());

        var polled = await client.GetFromJsonAsync<JsonDocument>(
            $"/api/v1/analyses/{analysisId}");
        Assert.Equal(
            "cancelled",
            polled!.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Confirmation_creates_inventory_only_for_accepted_detections()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
        var analysisId =
            Guid.Parse("50000000-0000-0000-0000-000000000001");

        using (var beforeScope = factory.Services.CreateScope())
        {
            var beforeContext = beforeScope.ServiceProvider.GetRequiredService<
                ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
            Assert.False(await beforeContext.Items.AnyAsync(
                item => item.DeduplicationKey ==
                    $"detection:{ApiFactory.DetectionId:N}"));
        }

        var response = await client.PostAsJsonAsync(
            $"/api/v1/analyses/{analysisId}/confirm",
            new
            {
                detections = new[]
                {
                    new
                    {
                        id = ApiFactory.DetectionId,
                        accepted = true,
                        name = "Packing tape",
                        description = "Corrected by the user",
                        category = "Supplies",
                        quantity = 2,
                        containerId = ApiFactory.ContainerId,
                        boundingBox = new
                        {
                            x = 0.2m,
                            y = 0.25m,
                            width = 0.3m,
                            height = 0.4m
                        }
                    }
                }
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<
            ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
        var item = await dbContext.Items
            .Include(candidate => candidate.Assignment)
            .SingleAsync(candidate =>
                candidate.DeduplicationKey ==
                $"detection:{ApiFactory.DetectionId:N}");
        Assert.Equal("Packing tape", item.Name);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(AssignmentStatus.Confirmed, item.Assignment.Status);
        Assert.Equal(ApiFactory.ContainerId, item.Assignment.ContainerId);
        Assert.NotNull(item.CropBlobName);
        Assert.Equal(0.2m, item.CropX);
        Assert.True(factory.PhotoStorage.Photos.ContainsKey(item.CropBlobName!));
        var detection = await dbContext.AnalysisDetections.SingleAsync(
            candidate => candidate.Id == ApiFactory.DetectionId);
        Assert.Equal(DetectionReviewStatus.Accepted, detection.ReviewStatus);
        Assert.Equal(item.Id, detection.ResultingItemId);
        Assert.Equal(0.2m, detection.ReviewedBoundingBoxX);

        using var cropClient = CreateAuthenticatedClient(
            "ItemOrganizer.Read",
            allowAutoRedirect: false);
        var cropResponse = await cropClient.GetAsync(
            $"/api/v1/items/{item.Id}/crop");
        Assert.Equal(HttpStatusCode.Redirect, cropResponse.StatusCode);
        Assert.Contains(
            Uri.EscapeDataString(item.CropBlobName!),
            cropResponse.Headers.Location!.ToString(),
            StringComparison.Ordinal);
        var cropContent = await client.GetAsync(
            $"/api/v1/items/{item.Id}/crop?content=true");
        Assert.Equal(HttpStatusCode.OK, cropContent.StatusCode);
        Assert.Equal(
            "image/png",
            cropContent.Content.Headers.ContentType?.MediaType);
        Assert.NotEmpty(await cropContent.Content.ReadAsByteArrayAsync());
    }

    [Fact]
    public async Task Crop_storage_failure_creates_no_inventory()
    {
        var photoId = Guid.NewGuid();
        var analysisId = Guid.NewGuid();
        var detectionId = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow.AddMinutes(-1);
        var blobName = $"tests/{photoId:N}.png";
        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<
                ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
            var photo = new Photo(
                photoId,
                ApiFactory.TenantId,
                ApiFactory.OwnerId,
                blobName,
                "image/png",
                1024,
                512,
                512,
                new string('d', 64),
                createdAt.AddDays(30),
                createdAt);
            var analysis = new Analysis(
                analysisId,
                ApiFactory.TenantId,
                ApiFactory.OwnerId,
                photoId,
                "prompt-v1",
                "schema-v1",
                "mock",
                "tests",
                null,
                createdAt);
            analysis.Start(createdAt.AddSeconds(1));
            analysis.Complete([], createdAt.AddSeconds(2));
            var detection = new AnalysisDetection(
                detectionId,
                ApiFactory.TenantId,
                ApiFactory.OwnerId,
                analysisId,
                "Storage box",
                null,
                "Tests",
                1,
                0.9m,
                null,
                new NormalizedBoundingBox(0.1m, 0.1m, 0.5m, 0.5m),
                createdAt.AddSeconds(2));
            dbContext.AddRange(photo, analysis, detection);
            await dbContext.SaveChangesAsync();
        }
        factory.PhotoStorage.Seed(
            blobName,
            CreatePhotoBytes(),
            "image/png");
        factory.PhotoStorage.FailUploads = true;
        try
        {
            using var client = CreateAuthenticatedClient(
                "ItemOrganizer.Read ItemOrganizer.Write ItemOrganizer.Analyze");
            var response = await client.PostAsJsonAsync(
                $"/api/v1/analyses/{analysisId}/confirm",
                new
                {
                    detections = new[]
                    {
                        new
                        {
                            id = detectionId,
                            accepted = true,
                            name = "Storage box",
                            description = (string?)null,
                            category = "Tests",
                            quantity = 1,
                            containerId = (Guid?)null,
                            boundingBox = new
                            {
                                x = 0.1m,
                                y = 0.1m,
                                width = 0.5m,
                                height = 0.5m
                            }
                        }
                    }
                });

            Assert.Equal(
                HttpStatusCode.ServiceUnavailable,
                response.StatusCode);
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<
                ItemOrganizer.Infrastructure.ItemOrganizerDbContext>();
            Assert.False(await dbContext.Items.AnyAsync(
                item => item.AnalysisId == analysisId));
            Assert.Equal(
                DetectionReviewStatus.Pending,
                (await dbContext.AnalysisDetections.SingleAsync(
                    detection => detection.Id == detectionId)).ReviewStatus);
        }
        finally
        {
            factory.PhotoStorage.FailUploads = false;
        }
    }

    private async Task<Guid> UploadPhotoAsync(HttpClient client)
    {
        using var content = CreatePhotoContent();
        var response = await client.PostAsync("/api/v1/photos", content);
        response.EnsureSuccessStatusCode();
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        return document!.RootElement.GetProperty("id").GetGuid();
    }

    private static MultipartFormDataContent CreatePhotoContent()
    {
        var content = new MultipartFormDataContent();
        var file = new ByteArrayContent(CreatePhotoBytes());
        file.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        content.Add(file, "file", "analysis.png");
        return content;
    }

    private static byte[] CreatePhotoBytes()
    {
        using var bitmap = new SKBitmap(512, 512);
        bitmap.Erase(SKColors.Green);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 90);
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

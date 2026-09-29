using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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

    private async Task<Guid> UploadPhotoAsync(HttpClient client)
    {
        using var bitmap = new SKBitmap(512, 512);
        bitmap.Erase(SKColors.Green);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 90);
        using var content = new MultipartFormDataContent();
        var file = new ByteArrayContent(data.ToArray());
        file.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        content.Add(file, "file", "analysis.png");

        var response = await client.PostAsync("/api/v1/photos", content);
        response.EnsureSuccessStatusCode();
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        return document!.RootElement.GetProperty("id").GetGuid();
    }

    private HttpClient CreateAuthenticatedClient(
        string scope,
        Guid? ownerId = null)
    {
        var client = factory.CreateClient();
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

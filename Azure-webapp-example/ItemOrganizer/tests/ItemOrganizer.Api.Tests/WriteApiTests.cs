using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ItemOrganizer.Api.Tests;

public sealed class WriteApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Write_route_requires_write_scope()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Read");

        var response = await client.PostAsJsonAsync(
            "/api/v1/containers",
            new { name = "Kitchen", labels = Array.Empty<string>() });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Manual_item_has_no_ai_provenance()
    {
        using var client = CreateAuthenticatedClient("ItemOrganizer.Write");

        var response = await client.PostAsJsonAsync(
            "/api/v1/items",
            new
            {
                name = "Packing tape",
                description = "Clear roll",
                category = "Supplies",
                quantity = 2
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.NotNull(response.Headers.ETag);
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            JsonValueKind.Null,
            document!.RootElement.GetProperty("photoId").ValueKind);
        Assert.Equal(
            JsonValueKind.Null,
            document.RootElement.GetProperty("analysisId").ValueKind);
        Assert.Equal(
            "unassigned",
            document.RootElement.GetProperty("assignmentStatus").GetString());
    }

    [Fact]
    public async Task Stale_container_update_returns_precondition_failed()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write");
        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"/api/v1/containers/{ApiFactory.ContainerId}")
        {
            Content = JsonContent.Create(new
            {
                name = "Renamed",
                description = "Changed",
                location = "Office",
                labels = Array.Empty<string>()
            })
        };
        request.Headers.TryAddWithoutValidation(
            "If-Match",
            "\"00000000-0000-0000-0000-000000000001\"");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);
    }

    [Fact]
    public async Task Non_empty_container_cannot_be_deleted()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write");
        var current = await client.GetAsync(
            $"/api/v1/containers/{ApiFactory.ContainerId}");
        current.EnsureSuccessStatusCode();
        using var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"/api/v1/containers/{ApiFactory.ContainerId}");
        request.Headers.IfMatch.Add(current.Headers.ETag!);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Manual_item_can_be_assigned_and_unassigned()
    {
        using var client = CreateAuthenticatedClient(
            "ItemOrganizer.Read ItemOrganizer.Write");
        var created = await client.PostAsJsonAsync(
            "/api/v1/items",
            new
            {
                name = $"Manual item {Guid.NewGuid():N}",
                category = "Test",
                quantity = 1
            });
        created.EnsureSuccessStatusCode();
        var createdDocument =
            await created.Content.ReadFromJsonAsync<JsonDocument>();
        var itemId = createdDocument!.RootElement.GetProperty("id").GetGuid();

        using var assign = new HttpRequestMessage(
            HttpMethod.Put,
            $"/api/v1/items/{itemId}/container")
        {
            Content = JsonContent.Create(new
            {
                containerId = ApiFactory.ContainerId,
                acceptSuggestion = false
            })
        };
        assign.Headers.IfMatch.Add(created.Headers.ETag!);
        var assigned = await client.SendAsync(assign);
        assigned.EnsureSuccessStatusCode();
        var assignedDocument =
            await assigned.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            "confirmed",
            assignedDocument!.RootElement
                .GetProperty("assignmentStatus")
                .GetString());

        using var unassign = new HttpRequestMessage(
            HttpMethod.Delete,
            $"/api/v1/items/{itemId}/container");
        unassign.Headers.IfMatch.Add(assigned.Headers.ETag!);
        var unassigned = await client.SendAsync(unassign);
        unassigned.EnsureSuccessStatusCode();
        var unassignedDocument =
            await unassigned.Content.ReadFromJsonAsync<JsonDocument>();
        Assert.Equal(
            "unassigned",
            unassignedDocument!.RootElement
                .GetProperty("assignmentStatus")
                .GetString());
    }

    private HttpClient CreateAuthenticatedClient(string scope)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add(
            "X-Test-Tenant",
            ApiFactory.TenantId.ToString());
        client.DefaultRequestHeaders.Add(
            "X-Test-Owner",
            ApiFactory.OwnerId.ToString());
        client.DefaultRequestHeaders.Add("X-Test-Scope", scope);
        return client;
    }
}

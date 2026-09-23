using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ItemOrganizer.Api.Tests;

public sealed class ReadApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Protected_read_without_token_returns_problem_details()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/containers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
        Assert.True(response.Headers.Contains("X-Correlation-ID"));
    }

    [Fact]
    public async Task Protected_read_without_scope_returns_forbidden()
    {
        using var client = CreateAuthenticatedClient(scope: "ItemOrganizer.Write");

        var response = await client.GetAsync("/api/v1/containers");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Cross_owner_resource_is_not_visible()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.GetAsync(
            $"/api/v1/containers/{ApiFactory.OtherContainerId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Container_read_returns_etag_and_utc_timestamp()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.GetAsync(
            $"/api/v1/containers/{ApiFactory.ContainerId}");

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Headers.ETag);
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var createdAt = DateTimeOffset.Parse(
            document!.RootElement.GetProperty("createdAt").GetString()!);
        Assert.Equal(TimeSpan.Zero, createdAt.Offset);
    }

    [Fact]
    public async Task Collection_paging_returns_continuation_token()
    {
        using var client = CreateAuthenticatedClient();

        var first = await client.GetFromJsonAsync<JsonDocument>(
            "/api/v1/containers?pageSize=1");
        var token = first!.RootElement
            .GetProperty("continuationToken")
            .GetString();

        Assert.NotNull(token);
        var second = await client.GetFromJsonAsync<JsonDocument>(
            $"/api/v1/containers?pageSize=1&continuationToken={token}");
        Assert.Single(second!.RootElement.GetProperty("items").EnumerateArray());
    }

    [Fact]
    public async Task Invalid_page_size_returns_problem_details()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.GetAsync("/api/v1/items?pageSize=101");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Health_endpoints_do_not_require_authentication()
    {
        using var client = factory.CreateClient();

        var live = await client.GetAsync("/api/v1/health/live");
        var ready = await client.GetAsync("/api/v1/health/ready");

        Assert.Equal(HttpStatusCode.OK, live.StatusCode);
        Assert.Equal(HttpStatusCode.OK, ready.StatusCode);
    }

    private HttpClient CreateAuthenticatedClient(
        string scope = "ItemOrganizer.Read")
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

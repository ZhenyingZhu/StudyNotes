using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Azure.Core;
using ItemOrganizer.Api;
using Microsoft.Extensions.Configuration;

namespace ItemOrganizer.Api.Tests;

public sealed class AzureOpenAiAnalysisProviderTests
{
    [Fact]
    public async Task Sends_private_photo_and_parses_structured_output()
    {
        var storage = new FakePhotoStorage();
        storage.Seed("photos/test", [1, 2, 3], "image/png");
        var handler = new RecordingHandler(
            HttpStatusCode.OK,
            CreateResponsePayload());
        var provider = CreateProvider(
            storage,
            handler,
            includeApiKey: true);

        var result = await provider.AnalyzeAsync(
            CreateRequest(),
            CancellationToken.None);

        Assert.Equal("USB-C cable", Assert.Single(result.Items).Name);
        Assert.Equal("test-key", handler.ApiKey);
        Assert.Null(handler.Authorization);
        Assert.Equal(
            "https://example.openai.azure.com/openai/v1/responses",
            handler.RequestUri?.ToString());
        using var request = JsonDocument.Parse(
            Assert.IsType<string>(handler.RequestBody));
        Assert.Equal(
            "gpt-5.4-mini",
            request.RootElement.GetProperty("model").GetString());
        var imageUrl = request.RootElement
            .GetProperty("input")[1]
            .GetProperty("content")[1]
            .GetProperty("image_url")
            .GetString();
        Assert.Equal("data:image/png;base64,AQID", imageUrl);
        Assert.True(request.RootElement
            .GetProperty("text")
            .GetProperty("format")
            .GetProperty("strict")
            .GetBoolean());
    }

    [Fact]
    public async Task Uses_bearer_token_when_api_key_is_absent()
    {
        var storage = new FakePhotoStorage();
        storage.Seed("photos/test", [1], "image/png");
        var handler = new RecordingHandler(
            HttpStatusCode.OK,
            CreateResponsePayload());
        var provider = CreateProvider(
            storage,
            handler,
            includeApiKey: false);

        await provider.AnalyzeAsync(
            CreateRequest(),
            CancellationToken.None);

        Assert.Equal("Bearer test-token", handler.Authorization);
        Assert.Null(handler.ApiKey);
    }

    [Fact]
    public async Task Throttling_is_a_transient_failure()
    {
        var storage = new FakePhotoStorage();
        storage.Seed("photos/test", [1], "image/png");
        var provider = CreateProvider(
            storage,
            new RecordingHandler(
                HttpStatusCode.TooManyRequests,
                """{"error":{"message":"throttled"}}"""),
            includeApiKey: true);

        await Assert.ThrowsAsync<AnalysisProviderTransientException>(
            () => provider.AnalyzeAsync(
                CreateRequest(),
                CancellationToken.None));
    }

    [Fact]
    public async Task Malformed_output_is_rejected_without_raw_payload()
    {
        var storage = new FakePhotoStorage();
        storage.Seed("photos/test", [1], "image/png");
        var provider = CreateProvider(
            storage,
            new RecordingHandler(
                HttpStatusCode.OK,
                """{"output_text":"not-json"}"""),
            includeApiKey: true);

        var exception =
            await Assert.ThrowsAsync<AnalysisProviderInvalidOutputException>(
                () => provider.AnalyzeAsync(
                    CreateRequest(),
                    CancellationToken.None));

        Assert.DoesNotContain("not-json", exception.Message);
    }

    [Theory]
    [InlineData(0.1, 0.2, 0.3, 0.4)]
    [InlineData(0.25, 0.5, 0.75, 0.5)]
    public async Task Valid_bounding_boxes_are_preserved(
        decimal x, decimal y, decimal width, decimal height)
    {
        var result = await AnalyzeBoundingBoxAsync(new
        {
            x, y, width, height
        });

        var box = Assert.Single(result.Items).BoundingBox;
        Assert.NotNull(box);
        Assert.Equal(x, box.X);
        Assert.Equal(y, box.Y);
        Assert.Equal(width, box.Width);
        Assert.Equal(height, box.Height);
        Assert.Empty(result.Warnings);
    }

    [Theory]
    [InlineData(0.8, 0.2, 0.3, 0.4)]
    [InlineData(0.1, 0.8, 0.3, 0.4)]
    [InlineData(-0.1, 0.2, 0.3, 0.4)]
    [InlineData(0.1, 0.2, 0, 0.4)]
    [InlineData(0.1, 0.2, 0.3, -0.4)]
    public async Task Invalid_bounding_boxes_preserve_detections_with_a_warning(
        decimal x, decimal y, decimal width, decimal height)
    {
        var result = await AnalyzeBoundingBoxAsync(new
        {
            x, y, width, height
        });

        var item = Assert.Single(result.Items);
        Assert.Equal("USB-C cable", item.Name);
        Assert.Null(item.BoundingBox);
        Assert.Contains("invalid bounding box", Assert.Single(result.Warnings));
    }

    [Fact]
    public async Task Null_bounding_box_does_not_add_a_warning()
    {
        var result = await AnalyzeBoundingBoxAsync(null);

        Assert.Null(Assert.Single(result.Items).BoundingBox);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public async Task Malformed_bounding_box_is_still_rejected()
    {
        await Assert.ThrowsAsync<AnalysisProviderInvalidOutputException>(
            () => AnalyzeBoundingBoxAsync(new
            {
                x = "not-a-number",
                y = 0.2m,
                width = 0.3m,
                height = 0.4m
            }));
    }

    private static async Task<AnalysisProviderResult> AnalyzeBoundingBoxAsync(
        object? boundingBox)
    {
        var envelope = JsonNode.Parse(CreateResponsePayload())!.AsObject();
        var output = JsonNode.Parse(
            envelope["output_text"]!.GetValue<string>())!.AsObject();
        output["items"]![0]!["boundingBox"] =
            JsonSerializer.SerializeToNode(boundingBox);
        envelope["output_text"] = output.ToJsonString();
        var storage = new FakePhotoStorage();
        storage.Seed("photos/test", [1, 2, 3], "image/png");
        var provider = CreateProvider(
            storage,
            new RecordingHandler(HttpStatusCode.OK, envelope.ToJsonString()),
            includeApiKey: true);

        return await provider.AnalyzeAsync(
            CreateRequest(),
            CancellationToken.None);
    }

    private static AzureOpenAiAnalysisProvider CreateProvider(
        FakePhotoStorage storage,
        HttpMessageHandler handler,
        bool includeApiKey)
    {
        var values = new Dictionary<string, string?>
        {
            ["AzureOpenAI:Endpoint"] =
                "https://example.openai.azure.com/"
        };
        if (includeApiKey)
        {
            values["AzureOpenAI:ApiKey"] = "test-key";
        }

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
        return new AzureOpenAiAnalysisProvider(
            new HttpClient(handler),
            storage,
            configuration,
            new TestCredential());
    }

    private static AnalysisProviderRequest CreateRequest()
    {
        return new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            "photos/test",
            "image/png",
            "2026-10-01.feasibility-1",
            "item-organizer.analysis-result.v1",
            "gpt-5.4-mini",
            [
                new(
                    Guid.Parse("30000000-0000-0000-0000-000000000001"),
                    "Cables",
                    "Cable storage",
                    "Office",
                    ["electronics"])
            ]);
    }

    private static string CreateResponsePayload()
    {
        var output = JsonSerializer.Serialize(new
        {
            schemaVersion = "item-organizer.analysis-result.v1",
            photoId = "20000000-0000-0000-0000-000000000001",
            promptVersion = "2026-10-01.feasibility-1",
            model = "gpt-5.4-mini",
            items = new[]
            {
                new
                {
                    name = "USB-C cable",
                    description = "Black braided cable",
                    category = "Electronics",
                    quantity = 1,
                    confidence = 0.93m,
                    suggestedContainerId =
                        "30000000-0000-0000-0000-000000000001",
                    suggestedContainerReason =
                        "The container is intended for cables."
                }
            },
            warnings = Array.Empty<string>()
        });
        return JsonSerializer.Serialize(new
        {
            output_text = output
        });
    }

    private sealed class RecordingHandler(
        HttpStatusCode statusCode,
        string responseBody) : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }

        public string? RequestBody { get; private set; }

        public string? ApiKey { get; private set; }

        public string? Authorization { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            RequestBody = request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);
            ApiKey = request.Headers.TryGetValues(
                "api-key",
                out var values)
                ? values.Single()
                : null;
            Authorization = request.Headers.Authorization?.ToString();
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(
                    responseBody,
                    Encoding.UTF8,
                    "application/json")
            };
        }
    }

    private sealed class TestCredential : TokenCredential
    {
        public override AccessToken GetToken(
            TokenRequestContext requestContext,
            CancellationToken cancellationToken)
        {
            return CreateToken();
        }

        public override ValueTask<AccessToken> GetTokenAsync(
            TokenRequestContext requestContext,
            CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(CreateToken());
        }

        private static AccessToken CreateToken() =>
            new(
                "test-token",
                DateTimeOffset.UtcNow.AddMinutes(5));
    }
}

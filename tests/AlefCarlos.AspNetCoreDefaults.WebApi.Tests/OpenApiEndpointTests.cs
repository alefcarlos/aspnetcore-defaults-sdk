using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Tests;

public class OpenApiEndpointTests : IClassFixture<WebApiFixture>
{
    private readonly HttpClient _client;

    public OpenApiEndpointTests(WebApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetOpenApiJson_ReturnsOk()
    {
        var response = await _client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(json.TryGetProperty("openapi", out _));
        Assert.True(json.TryGetProperty("info", out _));
    }
}

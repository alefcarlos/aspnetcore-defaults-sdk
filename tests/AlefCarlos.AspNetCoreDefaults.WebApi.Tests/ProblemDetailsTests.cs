using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Tests;

public class ProblemDetailsTests : IClassFixture<WebApiFixture>
{
    private readonly HttpClient _client;

    public ProblemDetailsTests(WebApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task ErrorEndpoint_ReturnsProblemDetails()
    {
        var response = await _client.GetAsync("/test/error", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(json.TryGetProperty("title", out _));
        Assert.True(json.TryGetProperty("status", out var status));
        Assert.Equal(500, status.GetInt32());
    }

    [Fact]
    public async Task NotFound_ReturnsProblemDetails()
    {
        var response = await _client.GetAsync("/nonexistent-route", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AlefCarlos.AspNetCoreDefaults.Worker.Tests;

public class AppInfoEndpointTests : IClassFixture<WorkerFixture>
{
    private readonly HttpClient _client;

    public AppInfoEndpointTests(WorkerFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetAppInfo_ReturnsOk()
    {
        var response = await _client.GetAsync("/app-info", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: TestContext.Current.CancellationToken);
        Assert.True(json.TryGetProperty("applicationName", out _));
        Assert.True(json.TryGetProperty("environmentName", out _));
        Assert.True(json.TryGetProperty("runtimeConfigurations", out _));
    }
}

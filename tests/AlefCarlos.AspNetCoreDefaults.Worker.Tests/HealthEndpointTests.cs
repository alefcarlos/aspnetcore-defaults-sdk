using System.Net;

namespace AlefCarlos.AspNetCoreDefaults.Worker.Tests;

public class HealthEndpointTests : IClassFixture<WorkerFixture>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(WorkerFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        var response = await _client.GetAsync("/health", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAlive_ReturnsOk()
    {
        var response = await _client.GetAsync("/alive", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

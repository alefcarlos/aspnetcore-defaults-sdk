using System.Net;

namespace AlefCarlos.AspNetCoreDefaults.Worker.Tests;

public class LogLevelManagementTests : IClassFixture<WorkerFixture>
{
    private readonly HttpClient _client;

    public LogLevelManagementTests(WorkerFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task LogLevelTest_ReturnsOk()
    {
        var response = await _client.PostAsync("/log-level:test", null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LogLevelChangeLevel_ReturnsNoContent()
    {
        var response = await _client.PostAsync("/log-level:changelevel?level=Warning", null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}

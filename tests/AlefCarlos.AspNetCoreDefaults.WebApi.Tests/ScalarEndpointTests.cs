using System.Net;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Tests;

public class ScalarEndpointTests : IClassFixture<WebApiFixture>
{
    private readonly HttpClient _client;

    public ScalarEndpointTests(WebApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetDocs_ReturnsHtml()
    {
        var response = await _client.GetAsync("/docs", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
    }
}

using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AlefCarlos.AspNetCoreDefaults.Worker.Tests;

public class ApplicationMetadataTests : IClassFixture<WorkerFixture>
{
    private readonly IServiceProvider _services;

    public ApplicationMetadataTests(WorkerFixture fixture)
    {
        _services = fixture.Services;
    }

    [Fact]
    public void ApplicationMetadata_IsRegistered()
    {
        var options = _services.GetRequiredService<IOptions<ApplicationMetadata>>();

        Assert.NotNull(options.Value);
        Assert.False(string.IsNullOrWhiteSpace(options.Value.ApplicationName));
        Assert.False(string.IsNullOrWhiteSpace(options.Value.EnvironmentName));
    }
}

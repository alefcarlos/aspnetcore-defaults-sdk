using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AlefCarlos.AspNetCoreDefaults.Worker.Tests;

public class HealthCheckTests : IClassFixture<WorkerFixture>
{
    private readonly IServiceProvider _services;

    public HealthCheckTests(WorkerFixture fixture)
    {
        _services = fixture.Services;
    }

    [Fact]
    public void HealthCheckService_IsRegistered()
    {
        var healthCheckService = _services.GetRequiredService<HealthCheckService>();

        Assert.NotNull(healthCheckService);
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        var healthCheckService = _services.GetRequiredService<HealthCheckService>();
        var result = await healthCheckService.CheckHealthAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }
}

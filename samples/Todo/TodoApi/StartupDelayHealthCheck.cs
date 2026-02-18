using Microsoft.Extensions.Diagnostics.HealthChecks;

public class StartupDelayHealthCheck : IHealthCheck
{
    private static readonly DateTime StartTime = DateTime.UtcNow;
    private static readonly TimeSpan DegradeAfter = TimeSpan.FromSeconds(30);

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var uptime = DateTime.UtcNow - StartTime;

        if (uptime > DegradeAfter)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    $"Aplicação rodando há {uptime.TotalSeconds:F0}s"
                ));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy(
                $"Aplicação iniciou há {uptime.TotalSeconds:F0}s"
            ));
    }
}

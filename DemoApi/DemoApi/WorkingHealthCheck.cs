using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoApi.HealthChecks
{
    public class WorkingHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }
    }
}

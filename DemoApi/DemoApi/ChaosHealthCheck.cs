using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DemoApi.HealthChecks
{
    public class ChaosHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Random random = new Random();
            int number = random.Next(0, 100);

            if (number <= 80)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "An unhealthy result."));
        }

    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesItAcademy.Api.Infrastructure.Healthchecks
{
    public class MovieHealthChecker : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var apiUrl = "https://localhost:44357/api/v1/";
            var client = new HttpClient { BaseAddress = new Uri(apiUrl) };

            HttpResponseMessage response = await client.GetAsync("Movie");

            HealthCheckResult healthCheckResult;
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.OK)
                healthCheckResult = await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Healthy));
            else
                healthCheckResult = await Task.FromResult(new HealthCheckResult(
                    status: HealthStatus.Unhealthy));

            return healthCheckResult;
        }
    }
}

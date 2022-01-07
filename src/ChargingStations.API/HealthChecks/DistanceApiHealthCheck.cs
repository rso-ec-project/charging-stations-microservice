using ChargingStations.Application.Shared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingStations.API.HealthChecks
{
    public class DistanceApiHealthCheck : IHealthCheck
    {
        private readonly DistanceCalculatorClient _distanceCalculatorClient;

        public DistanceApiHealthCheck(DistanceCalculatorClient distanceCalculatorClient)
        {
            _distanceCalculatorClient = distanceCalculatorClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _distanceCalculatorClient.Client.GetAsync("getdistance?start_lat=0.0&start_lng=0.0&end_lat=0.0&end_lng=0.0&unit=kilometers", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}

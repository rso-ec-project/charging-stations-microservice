using ChargingStations.Application.Shared;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingStations.API.HealthChecks
{
    public class EvUpdatesApiHealthCheck : IHealthCheck
    {
        private readonly ElectricVehicleUpdatesClient _electricVehicleUpdatesClient;

        public EvUpdatesApiHealthCheck(ElectricVehicleUpdatesClient electricVehicleUpdatesClient)
        {
            _electricVehicleUpdatesClient = electricVehicleUpdatesClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _electricVehicleUpdatesClient.Client.GetAsync("evupdates");

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}

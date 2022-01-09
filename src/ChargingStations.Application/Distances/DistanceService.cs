using ChargingStations.Application.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargingStations.Application.Distances
{
    public class DistanceService : IDistanceService
    {
        private readonly DistanceCalculatorClient _distanceCalculatorClient;
        private readonly ILogger<DistanceService> _logger;

        public DistanceService(DistanceCalculatorClient distanceCalculatorClient, ILogger<DistanceService> logger)
        {
            _distanceCalculatorClient = distanceCalculatorClient;
            _logger = logger;
        }

        public async Task<DistanceDto> GetAsync(double startLat, double startLong, double endLat, double endLong)
        {
            var endpoint = $"endpoint getdistance?start_lat={startLat}&start_lng={startLong}&end_lat={endLat}&end_lng={endLong}&unit=kilometers";
            try
            {
                _logger.LogInformation($"Entered Distance Calculator API {endpoint}");

                var responseMessage = _distanceCalculatorClient.Client.GetAsync($"getdistance?start_lat={startLat}&start_lng={startLong}&end_lat={endLat}&end_lng={endLong}&unit=kilometers").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                var distanceDto = await JsonSerializer.DeserializeAsync<DistanceDto>(responseStream, new JsonSerializerOptions()
                {
                    IgnoreReadOnlyProperties = true
                });

                _logger.LogInformation($"Exited Distance Calculator API {endpoint} with: 200 OK");
                return distanceDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited Distance Calculator API {endpoint} with: Exception {e}");
                return null;
            }
        }
    }
}

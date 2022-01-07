using ChargingStations.Application.Shared;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargingStations.Application.Distances
{
    public class DistanceService : IDistanceService
    {
        private readonly DistanceCalculatorClient _distanceCalculatorClient;

        public DistanceService(DistanceCalculatorClient distanceCalculatorClient)
        {
            _distanceCalculatorClient = distanceCalculatorClient;
        }

        public async Task<DistanceDto> GetAsync(double startLat, double startLong, double endLat, double endLong)
        {
            try
            {
                var responseMessage = _distanceCalculatorClient.Client.GetAsync($"getdistance?start_lat={startLat}&start_lng={startLong}&end_lat={endLat}&end_lng={endLong}&unit=kilometers").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<DistanceDto>(responseStream, new JsonSerializerOptions()
                {
                    IgnoreReadOnlyProperties = true
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}

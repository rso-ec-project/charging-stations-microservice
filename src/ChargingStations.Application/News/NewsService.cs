using ChargingStations.Application.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargingStations.Application.News
{
    public class NewsService : INewsService
    {
        private readonly ElectricVehicleUpdatesClient _electricVehicleUpdatesClient;

        public NewsService(ElectricVehicleUpdatesClient electricVehicleUpdatesClient)
        {
            _electricVehicleUpdatesClient = electricVehicleUpdatesClient;
        }

        public async Task<List<NewsDto>> GetAsync()
        {
            try
            {
                var responseMessage = _electricVehicleUpdatesClient.Client.GetAsync("evupdates").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<List<NewsDto>>(responseStream, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

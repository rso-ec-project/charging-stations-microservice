using ChargingStations.Application.Shared;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NewsService> _logger;

        public NewsService(ElectricVehicleUpdatesClient electricVehicleUpdatesClient, ILogger<NewsService> logger)
        {
            _electricVehicleUpdatesClient = electricVehicleUpdatesClient;
            _logger = logger;
        }

        public async Task<List<NewsDto>> GetAsync()
        {
            var endpoint = $"endpoint evupdates";
            try
            {
                _logger.LogInformation($"Entered EV Updates API {endpoint}");

                var responseMessage = _electricVehicleUpdatesClient.Client.GetAsync("evupdates").Result;

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                var newsDtos = await JsonSerializer.DeserializeAsync<List<NewsDto>>(responseStream, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                _logger.LogInformation($"Exited EV Updates API {endpoint} with: 200 OK");
                return newsDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited EV Updates API {endpoint} with: Exception {e}");
                throw;
            }
        }
    }
}

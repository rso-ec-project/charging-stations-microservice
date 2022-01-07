using ChargingStations.Application.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargingStations.Application.CommentsMicroservice.Ratings
{
    public class RatingService : IRatingService
    {
        private readonly CommentsMicroServiceClient _commentsMicroServiceClient;
        private readonly ILogger<RatingService> _logger;

        public RatingService(CommentsMicroServiceClient commentsMicroServiceClient, ILogger<RatingService> logger)
        {
            _commentsMicroServiceClient = commentsMicroServiceClient;
            _logger = logger;
        }

        public async Task<RatingDto> GetAsync(int chargingStationId)
        {
            var endpoint = $"endpoint Ratings?chargingStationId={chargingStationId}";
            try
            {
                _logger.LogInformation($"Entered Comments MS {endpoint}");
                var responseMessage = _commentsMicroServiceClient.Client.GetAsync($"Ratings?chargingStationId={chargingStationId}").Result;

                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation($"Exited Comments MS {endpoint} with: 404 Ratings for ChargingStation with Id {chargingStationId} not found");
                    return new RatingDto();
                }

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                var ratingDtos = await JsonSerializer.DeserializeAsync<RatingDto>(responseStream);

                _logger.LogInformation($"Exited Comments MS {endpoint} with: 200 OK");
                return ratingDtos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exited Comments MS {endpoint} with: Exception {e.Message}");
                return null;
            }
        }
    }
}

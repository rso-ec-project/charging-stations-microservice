using ChargingStations.Application.Shared;
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
        private readonly IServiceDiscoveryClient _serviceDiscoveryClient;
        private const string ServiceName = "comments-ms";

        public RatingService(CommentsMicroServiceClient commentsMicroServiceClient, IServiceDiscoveryClient serviceDiscoveryClient)
        {
            _commentsMicroServiceClient = commentsMicroServiceClient;
            _serviceDiscoveryClient = serviceDiscoveryClient;
        }

        public async Task<RatingDto> GetAsync(int chargingStationId)
        {
            try
            {
                _commentsMicroServiceClient.Client.BaseAddress = await _serviceDiscoveryClient.GetRequestUriAsync(ServiceName);
                var responseMessage = _commentsMicroServiceClient.Client.GetAsync($"Ratings?chargingStationId={chargingStationId}").Result;

                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return new RatingDto();
                }

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(responseMessage.StatusCode.ToString());
                }

                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<RatingDto>(responseStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}

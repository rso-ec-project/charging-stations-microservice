using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class ReservationsMicroServiceClient
    {
        public HttpClient Client { get; set; }

        public ReservationsMicroServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var commentsServiceEnv = configuration["ReservationsService:Environment"].Replace("\"", "");
            Client = httpClientFactory.CreateClient(commentsServiceEnv.Equals("dev") ? "reservations-dev" : "reservations");
        }
    }
}

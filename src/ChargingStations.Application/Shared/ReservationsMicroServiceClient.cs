using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class ReservationsMicroServiceClient
    {
        public HttpClient Client { get; set; }

        public ReservationsMicroServiceClient(HttpClient client)
        {
            Client = client;
        }
    }
}

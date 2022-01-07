using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class DistanceCalculatorClient
    {
        public HttpClient Client;

        public DistanceCalculatorClient(HttpClient client)
        {
            Client = client;
        }
    }
}

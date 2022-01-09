using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class ElectricVehicleUpdatesClient
    {
        public HttpClient Client;

        public ElectricVehicleUpdatesClient(HttpClient client)
        {
            Client = client;
        }
    }
}

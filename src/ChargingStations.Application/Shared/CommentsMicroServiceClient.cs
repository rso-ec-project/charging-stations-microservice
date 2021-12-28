using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class CommentsMicroServiceClient
    {
        public HttpClient Client { get; set; }

        public CommentsMicroServiceClient(HttpClient client)
        {
            Client = client;
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ChargingStations.Application.Shared
{
    public class CommentsMicroServiceClient
    {
        public HttpClient Client { get; set; }

        public CommentsMicroServiceClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            var commentsServiceEnv = configuration["CommentsService:Environment"].Replace("\"", "");
            Client = httpClientFactory.CreateClient(commentsServiceEnv.Equals("dev") ? "comments-dev" : "comments");
        }
    }
}

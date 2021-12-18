using System.Threading.Tasks;

namespace ChargingStations.Application.CommentsMicroservice.Ratings
{
    public interface IRatingService
    {
        Task<RatingDto> GetAsync(int chargingStationId);
    }
}

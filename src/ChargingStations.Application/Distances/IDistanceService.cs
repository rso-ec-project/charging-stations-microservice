using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.Distances
{
    public interface IDistanceService
    {
        Task<DistanceDto> GetAsync(double startLat, double startLong, double endLat, double endLong);
    }
}

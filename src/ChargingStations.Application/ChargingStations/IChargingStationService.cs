using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargingStations
{
    public interface IChargingStationService
    {
        Task<List<ChargingStationDto>> GetAsync();
    }
}

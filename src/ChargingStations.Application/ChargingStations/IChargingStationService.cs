using ChargingStations.Application.Chargers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargingStations
{
    public interface IChargingStationService
    {
        Task<List<ChargingStationDto>> GetAsync(double latitude, double longitude);
        Task<ChargingStationDto> GetAsync(int chargingStationId, double latitude, double longitude);
        Task<List<ChargerDto>> GetChargersAsync(int chargingStationId);
    }
}

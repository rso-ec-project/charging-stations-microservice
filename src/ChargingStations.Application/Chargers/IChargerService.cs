using System.Threading.Tasks;

namespace ChargingStations.Application.Chargers
{
    public interface IChargerService
    {
        Task<ChargerDto> GetAsync(int chargerId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.ChargerModels
{
    public interface IChargerModelService
    {
        Task<List<ChargerModelDto>> GetAsync();
    }
}

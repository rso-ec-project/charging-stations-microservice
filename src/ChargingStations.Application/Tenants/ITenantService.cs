using System.Collections.Generic;
using System.Threading.Tasks;
using ChargingStations.Application.ChargingStations;

namespace ChargingStations.Application.Tenants
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAsync();
        Task<TenantDto> GetAsync(int tenantId);
        Task<List<ChargingStationDto>> GetChargingStationsAsync(int tenantId);
    }
}

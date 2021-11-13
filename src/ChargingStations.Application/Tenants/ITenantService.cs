using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStations.Application.Tenants
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAsync();
        Task<TenantDto> GetAsync(int id);
    }
}

using ChargingStations.Domain.Shared;

namespace ChargingStations.Domain.TenantAggregate
{
    public interface ITenantRepository : IRepository<Tenant, int>
    {
    }
}

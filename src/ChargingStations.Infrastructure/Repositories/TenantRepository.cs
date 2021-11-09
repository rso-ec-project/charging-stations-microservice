using ChargingStations.Domain.TenantAggregate;

namespace ChargingStations.Infrastructure.Repositories
{
    public class TenantRepository : Repository<Tenant, int>, ITenantRepository
    {
        public TenantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

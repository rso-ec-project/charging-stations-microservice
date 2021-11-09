using ChargingStations.Domain.ChargerAggregate;

namespace ChargingStations.Infrastructure.Repositories
{
    public class ChargerRepository : Repository<Charger, int>, IChargerRepository
    {
        public ChargerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

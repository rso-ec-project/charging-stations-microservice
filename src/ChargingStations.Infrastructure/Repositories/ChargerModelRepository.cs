using ChargingStations.Domain.ChargerModelAggregate;

namespace ChargingStations.Infrastructure.Repositories
{
    public class ChargerModelRepository : Repository<ChargerModel, int>, IChargerModelRepository
    {
        public ChargerModelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

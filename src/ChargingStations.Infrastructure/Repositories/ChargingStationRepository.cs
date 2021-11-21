using ChargingStations.Domain.ChargingStationAggregate;

namespace ChargingStations.Infrastructure.Repositories
{
    public class ChargingStationRepository : Repository<ChargingStation, int>, IChargingStationRepository
    {
        public ChargingStationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

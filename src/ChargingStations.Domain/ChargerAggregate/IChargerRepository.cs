using ChargingStations.Domain.Shared;

namespace ChargingStations.Domain.ChargerAggregate
{
    public interface IChargerRepository : IRepository<Charger, int>
    {
    }
}

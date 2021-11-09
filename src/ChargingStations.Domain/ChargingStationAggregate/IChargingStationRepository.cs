using ChargingStations.Domain.Shared;

namespace ChargingStations.Domain.ChargingStationAggregate
{
    public interface IChargingStationRepository : IRepository<ChargingStation, int>
    {
    }
}

using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared.Entities;

namespace ChargingStations.Domain.ChargerAggregate
{
    public class Charger : Entity<int>
    {
        public string Name { get; set; }
        public double ChargingFeePerKwh { get; set; }
        public int ChargingStationId { get; set; }
        public int ChargerModelId { get; set; }

        public virtual ChargingStation ChargingStation { get; set; }
        public virtual ChargerModel ChargerModel { get; set; }
    }
}

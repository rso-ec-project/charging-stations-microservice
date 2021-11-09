using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;

namespace ChargingStations.Domain.ChargerAggregate
{
    public class Charger
    {
        public int ChargerId { get; set; }
        public string Name { get; set; }
        public double ChargingFeePerKwh { get; set; }
        public int ChargingStationId { get; set; }
        public int ChargerModelId { get; set; }

        public virtual ChargingStation ChargingStation { get; set; }
        public virtual ChargerModel ChargerModel { get; set; }
    }
}

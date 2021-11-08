namespace ChargingStations.Domain.Entities
{
    public class Charger
    {
        public int ChargerId { get; set; }
        public string Name { get; set; }
        public int ChargingStationId { get; set; }
        public int ChargerModelId { get; set; }

        public virtual ChargingStation ChargingStation { get; set; }
        public virtual ChargerModel ChargerModel { get; set; }
    }
}

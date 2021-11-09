using ChargingStations.Domain.ChargerAggregate;
using System.Collections.Generic;

namespace ChargingStations.Domain.ChargerModelAggregate
{
    public class ChargerModel
    {
        public int ChargerModelId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }

        public virtual ICollection<Charger> Chargers { get; set; }
    }
}

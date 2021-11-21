using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.Shared.Entities;
using System.Collections.Generic;

namespace ChargingStations.Domain.ChargerModelAggregate
{
    public class ChargerModel : Entity<int>
    {
        public int ChargerModelId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }

        public virtual ICollection<Charger> Chargers { get; set; }
    }
}

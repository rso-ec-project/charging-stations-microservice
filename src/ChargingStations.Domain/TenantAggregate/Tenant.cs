using ChargingStations.Domain.ChargingStationAggregate;
using System.Collections.Generic;

namespace ChargingStations.Domain.TenantAggregate
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ChargingStation> ChargingStations { get; set; }
    }
}

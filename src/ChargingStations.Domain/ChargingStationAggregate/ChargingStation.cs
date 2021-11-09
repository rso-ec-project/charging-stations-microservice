using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.Shared.Entities;
using ChargingStations.Domain.TenantAggregate;
using System.Collections.Generic;

namespace ChargingStations.Domain.ChargingStationAggregate
{
    public class ChargingStation : Entity<int>
    {
        public int ChargingStationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TenantId { get; set; }

        public virtual ICollection<Charger> Chargers { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}

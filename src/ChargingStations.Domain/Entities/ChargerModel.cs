using System.Collections.Generic;

namespace ChargingStations.Domain.Entities
{
    public class ChargerModel
    {
        public int ChargerModelId { get; set; }
        public string Name { get; set; }
        public double ChargingFeePerKwh { get; set; }
        public string Manufacturer { get; set; }
        public int TenantId { get; set; }

        public virtual ICollection<Charger> Chargers { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}

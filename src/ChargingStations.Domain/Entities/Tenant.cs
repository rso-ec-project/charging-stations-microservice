using System.Collections.Generic;

namespace ChargingStations.Domain.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ChargingStation> ChargingStations { get; set; }
        public virtual ICollection<ChargerModel> ChargerModels { get; set; }
    }
}

using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.TenantAggregate;
using System.Threading.Tasks;

namespace ChargingStations.Domain.Shared
{
    public interface IUnitOfWork
    {
        IUnitOfWork CreateContext();

        IChargerRepository ChargerRepository { get; }
        
        IChargerModelRepository ChargerModelRepository { get; }

        IChargingStationRepository ChargingStationRepository { get; }

        ITenantRepository TenantRepository { get; }

        Task<int> CommitAsync();
    }
}

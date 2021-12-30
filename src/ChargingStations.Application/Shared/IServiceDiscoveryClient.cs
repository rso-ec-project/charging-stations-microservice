using System;
using System.Threading.Tasks;

namespace ChargingStations.Application.Shared
{
    public interface IServiceDiscoveryClient
    {
        Task<Uri> GetRequestUriAsync(string serviceName);
    }
}

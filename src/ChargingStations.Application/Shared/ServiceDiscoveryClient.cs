using Consul;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStations.Application.Shared
{
    public class ServiceDiscoveryClient : IServiceDiscoveryClient
    {
        private readonly IConsulClient _consulClient;

        public ServiceDiscoveryClient(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public async Task<Uri> GetRequestUriAsync(string serviceName)
        {
            var allRegisteredServices = await _consulClient.Agent.Services();
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            var service = registeredServices.FirstOrDefault();

            if (service == null)
            {
                throw new Exception($"Consul service: '{serviceName}' was not found.");
            }

            return new Uri($"{service.Address}:{service.Port}/api/v1/");
        }
    }
}

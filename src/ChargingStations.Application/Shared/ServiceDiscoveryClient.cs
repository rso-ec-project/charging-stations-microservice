using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;

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

            var service = GetRandomInstance(registeredServices, serviceName);

            if (service == null)
            {
                throw new Exception($"Consul service: '{serviceName}' was not found.");
            }

            return new Uri($"{service.Address}/api/v1/");
        }

        private static AgentService GetRandomInstance(IList<AgentService> services, string serviceName)
        {
            var random = new Random();

            var serviceToUse = services[random.Next(0, services.Count)];

            return serviceToUse;
        }
    }
}

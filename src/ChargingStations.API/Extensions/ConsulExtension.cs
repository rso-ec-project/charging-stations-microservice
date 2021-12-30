using ChargingStations.Application.Shared;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChargingStations.API.Extensions
{
    public static class ConsulExtension
    {
        public static IServiceCollection AddConsul(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IServiceDiscoveryClient, ServiceDiscoveryClient>();

            var host = configuration["ServiceDiscovery:Consul:Host"];

            return serviceCollection.AddSingleton<IConsulClient>(_ => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(host))
                {
                    cfg.Address = new Uri(host);
                }
            }));
        }
    }
}

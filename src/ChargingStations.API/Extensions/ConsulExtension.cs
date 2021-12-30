using ChargingStations.Application.Shared;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public static void UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            if (configuration == null)
                return;

            var service = configuration["ServiceDiscovery:ServiceName"];
            var address = configuration["ServiceDiscovery:Consul:Address"];
            var port = int.Parse(configuration["ServiceDiscovery:Consul:Port"]);

            var client = scope.ServiceProvider.GetService<IConsulClient>();

            var consulServiceId = $"{service}:{Guid.NewGuid()}";

            var consulServiceRegistration = new AgentServiceRegistration
            {
                Name = service,
                ID = consulServiceId,
                Address = address,
                Port = port
            };

            client?.Agent.ServiceRegister(consulServiceRegistration);

            lifetime.ApplicationStopping.Register(() => {
                client?.Agent.ServiceDeregister(consulServiceRegistration.ID).Wait();
            });
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Winton.Extensions.Configuration.Consul;
using Winton.Extensions.Configuration.Consul.Parsers;

namespace ChargingStations.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (_, builder) =>
                    {
                        var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false)
                            .Build();

                        builder.AddConsul(
                            "ChargingStationsMicroservice",
                            options =>
                            {
                                options.ConsulConfigurationOptions =
                                    cco => { cco.Address = new Uri(Environment.GetEnvironmentVariable("CONSUL_HTTP_ADDR") ?? string.Empty); };
                                options.Optional = false;
                                options.ReloadOnChange = true;
                                options.Parser = new SimpleConfigurationParser();
                            });
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

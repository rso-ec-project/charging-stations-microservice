using AutoMapper;
using ChargingStations.API.Extensions;
using ChargingStations.Application.ChargerModels;
using ChargingStations.Application.Chargers;
using ChargingStations.Application.ChargingStations;
using ChargingStations.Application.CommentsMicroservice.Ratings;
using ChargingStations.Application.ReservationsMicroService.ReservationSlots;
using ChargingStations.Application.Shared;
using ChargingStations.Application.Tenants;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using ChargingStations.Infrastructure;
using ChargingStations.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ChargingStations.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseNpgsql(GetConnectionString());
            });

            var mapperConfig = CreateMapperConfiguration();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IChargerService, ChargerService>();
            services.AddScoped<IChargerModelService, ChargerModelService>();
            services.AddScoped<IChargingStationService, ChargingStationService>();
            services.AddScoped<ITenantService, TenantService>();

            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IReservationSlotService, ReservationSlotService>();

            services.AddScoped<IChargerRepository, ChargerRepository>();
            services.AddScoped<IChargerModelRepository, ChargerModelRepository>();
            services.AddScoped<IChargingStationRepository, ChargingStationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddHttpClient<CommentsMicroServiceClient>((_, client) =>
                {
                    SetHttpClientBaseAddress(client, new Uri(Configuration["ApplicationSettings:CommentsMSAddress"]));
                    SetHttpClientRequestHeader(client, "ChargingStationsMS");
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                    new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                );

            services.AddHttpClient<ReservationsMicroServiceClient>((_, client) =>
                {
                    SetHttpClientBaseAddress(client, new Uri(Configuration["ApplicationSettings:ReservationsMSAddress"]));
                    SetHttpClientRequestHeader(client, "ChargingStationsMS");
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                    new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }
                );

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(tags: new[] { "ready" });

            services.AddControllers();
            services.AddSwagger();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        private static void SetHttpClientRequestHeader(HttpClient client, string userAgent)
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            client.DefaultRequestVersion = new Version(1, 0);
        }

        private static void SetHttpClientBaseAddress(HttpClient client, Uri baseAddress)
        {
            client.BaseAddress = baseAddress;
        }

        private static string GetConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USERNAME");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            return $"Host={host};Database={database};Username={username};Password={password}";
        }

        private static MapperConfiguration CreateMapperConfiguration()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ChargerModelMapperProfile());
                mc.AddProfile(new ChargingStationMapperProfile());
                mc.AddProfile(new ChargerMapperProfile());
                mc.AddProfile(new TenantMapperProfile());
            });

            return mapperConfig;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChargingStations.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready")
                });
                endpoints.MapHealthChecks("health/live", new HealthCheckOptions()
                {
                    Predicate = _ => false
                });
            });
        }
    }
}

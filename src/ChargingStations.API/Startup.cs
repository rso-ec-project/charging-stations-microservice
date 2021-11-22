using AutoMapper;
using ChargingStations.Application.ChargerModels;
using ChargingStations.Application.Chargers;
using ChargingStations.Application.ChargingStations;
using ChargingStations.Application.Tenants;
using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.Shared;
using ChargingStations.Domain.TenantAggregate;
using ChargingStations.Infrastructure;
using ChargingStations.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                options.UseNpgsql(FormatConnectionString(Configuration.GetSection("ConnectionString").Value));
            });

            var mapperConfig = CreateMapperConfiguration();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IChargerService, ChargerService>();
            services.AddScoped<IChargerModelService, ChargerModelService>();
            services.AddScoped<IChargingStationService, ChargingStationService>();
            services.AddScoped<ITenantService, TenantService>();

            services.AddScoped<IChargerRepository, ChargerRepository>();
            services.AddScoped<IChargerModelRepository, ChargerModelRepository>();
            services.AddScoped<IChargingStationRepository, ChargingStationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChargingStations.API", Version = "v1" });
            });
        }

        private static string FormatConnectionString(string connectionString)
        {
            return connectionString.Replace("\"", "");
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChargingStations.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

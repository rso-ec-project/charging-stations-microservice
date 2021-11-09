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
                options.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IChargerRepository, ChargerRepository>();
            services.AddScoped<IChargerModelRepository, ChargerModelRepository>();
            services.AddScoped<IChargingStationRepository, ChargingStationRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.1", new OpenApiInfo { Title = "ChargingStations.API", Version = "v0.1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v0.1/swagger.json", "ChargingStations.API v0.1"));
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

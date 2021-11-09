using ChargingStations.Domain.ChargerAggregate;
using ChargingStations.Domain.ChargerModelAggregate;
using ChargingStations.Domain.ChargingStationAggregate;
using ChargingStations.Domain.TenantAggregate;
using ChargingStations.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChargingStations.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<ChargingStation> ChargingStations { get; set; }
        public DbSet<ChargerModel> ChargerModels { get; set; }
        public DbSet<Charger> Chargers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChargerModelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChargingStationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChargerModelEntityTypeConfiguration());
        }
    }
}

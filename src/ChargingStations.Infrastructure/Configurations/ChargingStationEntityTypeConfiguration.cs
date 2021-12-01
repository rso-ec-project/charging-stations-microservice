using ChargingStations.Domain.ChargingStationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChargingStations.Infrastructure.Configurations
{
    public class ChargingStationEntityTypeConfiguration : IEntityTypeConfiguration<ChargingStation>
    {
        public void Configure(EntityTypeBuilder<ChargingStation> builder)
        {
            builder.ToTable("charging_station");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Address)
                .HasColumnName("address")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Latitude)
                .HasColumnName("latitude")
                .IsRequired();

            builder.Property(x => x.Longitude)
                .HasColumnName("longitude")
                .IsRequired();

            builder.HasOne(x => x.Tenant)
                .WithMany(y => y.ChargingStations)
                .HasForeignKey(z => z.TenantId)
                .HasConstraintName("FK_ChargingStation_Tenant_TenantId");

            builder.Property(x => x.TenantId)
                .HasColumnName("tenant_id")
                .IsRequired();
        }
    }
}

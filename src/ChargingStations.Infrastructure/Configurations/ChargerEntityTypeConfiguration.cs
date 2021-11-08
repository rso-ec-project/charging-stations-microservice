using ChargingStations.Domain.ChargerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChargingStations.Infrastructure.Configurations
{
    public class ChargerEntityTypeConfiguration : IEntityTypeConfiguration<Charger>
    {
        public void Configure(EntityTypeBuilder<Charger> builder)
        {
            builder.ToTable("charger");

            builder.HasKey(x => x.ChargerId);

            builder.Property(x => x.ChargerId)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.ChargingFeePerKwh)
                .IsRequired()
                .HasColumnName("charging_fee_per_kwh");

            builder.HasOne(x => x.ChargingStation)
                .WithMany(y => y.Chargers)
                .HasForeignKey(z => z.ChargingStationId)
                .HasConstraintName("FK_Charger_ChargingStation_ChargingStationId");

            builder.Property(x => x.ChargingStationId)
                .HasColumnName("charging_station_id")
                .IsRequired();

            builder.HasOne(x => x.ChargerModel)
                .WithMany(y => y.Chargers)
                .HasForeignKey(z => z.ChargerId)
                .HasConstraintName("FK_Charger_ChargerModel_ChargerModelId");

            builder.Property(x => x.ChargerModelId)
                .HasColumnName("charger_model_id")
                .IsRequired();
        }
    }
}

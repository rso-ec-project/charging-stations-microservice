using ChargingStations.Domain.ChargerModelAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChargingStations.Infrastructure.Configurations
{
    public class ChargerModelEntityTypeConfiguration : IEntityTypeConfiguration<ChargerModel>
    {
        public void Configure(EntityTypeBuilder<ChargerModel> builder)
        {
            builder.ToTable("charger_model");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(x => x.Manufacturer)
                .HasColumnName("manufacturer")
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}

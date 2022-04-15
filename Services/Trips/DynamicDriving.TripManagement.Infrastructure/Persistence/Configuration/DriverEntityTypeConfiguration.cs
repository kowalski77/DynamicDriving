using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence.Configuration;

public class DriverEntityTypeConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        Guards.ThrowIfNull(builder);

        builder.OwnsOne(x => x.CurrentCoordinates, y =>
        {
            y.Property(z => z.Latitude).HasColumnName("Latitude");
            y.Property(z => z.Longitude).HasColumnName("Longitude");
        });
    }
}

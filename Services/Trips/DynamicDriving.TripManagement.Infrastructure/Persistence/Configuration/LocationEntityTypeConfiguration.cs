using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence.Configuration;

public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        Guards.ThrowIfNull(builder);

        builder.OwnsOne(x => x.Coordinates, y =>
        {
            y.Property(x => x.Latitude).HasColumnName("Latitude");
            y.Property(x => x.Longitude).HasColumnName("Longitude");
        });
    }
}

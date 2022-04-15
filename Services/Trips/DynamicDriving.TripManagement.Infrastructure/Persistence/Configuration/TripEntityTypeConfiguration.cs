using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence.Configuration;

public class TripEntityTypeConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        Guards.ThrowIfNull(builder);

        builder.OwnsOne(x => x.UserId, y =>
        {
            y.Property(z => z.Value).HasColumnName("UserId");
        });

        builder.OwnsOne(x => x.CurrentCoordinates, y =>
        {
            y.Property(z => z.Latitude).HasColumnName("Latitude");
            y.Property(z => z.Longitude).HasColumnName("Longitude");
        });

        builder
            .HasOne(x => x.Driver)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Destination)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Origin)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}

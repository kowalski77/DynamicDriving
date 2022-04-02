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

        builder.HasOne(x => x.Destination)
            .WithOne()
            .HasForeignKey<Trip>("DestinationId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}

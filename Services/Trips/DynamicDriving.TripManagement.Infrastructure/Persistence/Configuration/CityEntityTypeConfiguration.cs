using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence.Configuration;

public class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        Guards.ThrowIfNull(builder);

        builder.HasData(GetInitialCities());
    }

    private static IEnumerable<City> GetInitialCities() => new[] { new City("Sabadell"), new City("Barcelona") };
}

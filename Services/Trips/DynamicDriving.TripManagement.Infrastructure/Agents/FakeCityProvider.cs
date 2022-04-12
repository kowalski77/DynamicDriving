using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public sealed class FakeCityProvider : ICityProvider
{
    // TODO: Fake Agent Service, replace with Google API
    public Task<Maybe<City>> GetCityByCoordinatesAsync(Coordinates coordinates)
    {
        Guards.ThrowIfNull(coordinates);

        if (coordinates.Latitude > 0)
        {
            return Task.FromResult((Maybe<City>)new City("Barcelona"));
        }

        return Task.FromResult((Maybe<City>)new City("Sabadell"));
    }
}

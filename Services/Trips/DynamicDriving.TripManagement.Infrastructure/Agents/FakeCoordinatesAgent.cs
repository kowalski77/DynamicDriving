using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public sealed class FakeCoordinatesAgent : ICoordinatesAgent
{
    // TODO: Fake Agent Service, dummy implementations, replace with Google API; since 3rd party agent, handle exceptions and timeouts
    public Task<Result<City>> GetCityByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(coordinates);

        if (coordinates.Latitude > 0)
        {
            return Task.FromResult(Result.Ok(new City("Barcelona")));
        }

        return Task.FromResult(Result.Ok(new City("Sabadell")));
    }

    public Task<Result<Location>> GetLocationByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(coordinates);

        if (coordinates.Latitude > 0)
        {
            return Task.FromResult(Result.Ok(new Location(Guid.NewGuid(), "Location 1", new City("Barcelona"), coordinates)));
        }

        return Task.FromResult(Result.Ok(new Location(Guid.NewGuid(), "Location 2", new City("Sabadell"), coordinates)));
    }
}

using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public class LocationService : ILocationService
{
    private readonly ILocationProvider locationProvider;
    private readonly ILocationRepository locationRepository;

    public LocationService(ILocationRepository locationRepository, ILocationProvider locationProvider)
    {
        this.locationRepository = Guards.ThrowIfNull(locationRepository);
        this.locationProvider = Guards.ThrowIfNull(locationProvider);
    }

    public async Task<Result<Location>> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        var maybeLocation = await this.locationProvider.GetLocationAsync(coordinates);
        if (!maybeLocation.TryGetValue(out var location))
        {
            return Result.Fail<Location>(LocationErrors.InvalidCoordinates(coordinates.Latitude, coordinates.Longitude));
        }

        var currentLocations = await this.locationRepository.GetLocationsAsync(cancellationToken);

        return currentLocations.Any(loc => loc.IsPermittedArea(location))
            ? Result.Ok(location) :
            Result.Fail<Location>(LocationErrors.InvalidCityCoordinates(coordinates.Latitude, coordinates.Longitude));
    }
}

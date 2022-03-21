using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public class CoordinatesValidator : ICoordinatesValidator
{
    private readonly ILocationProvider locationProvider;
    private readonly ILocationRepository locationRepository;

    public CoordinatesValidator(ILocationRepository locationRepository, ILocationProvider locationProvider)
    {
        this.locationRepository = Guards.ThrowIfNull(locationRepository);
        this.locationProvider = Guards.ThrowIfNull(locationProvider);
    }

    public async Task<Result> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        var maybeLocation = await this.locationProvider.GetLocationAsync(coordinates).ConfigureAwait(false);
        if (!maybeLocation.TryGetValue(out var location))
        {
            return Result.Fail(LocationErrors.InvalidCoordinates(coordinates.Latitude, coordinates.Longitude));
        }

        var currentLocations = await this.locationRepository.GetLocationsAsync(cancellationToken).ConfigureAwait(false);
        return currentLocations.Contains(location) ? 
            Result.Ok() : 
            Result.Fail(LocationErrors.InvalidCityCoordinates(coordinates.Latitude, coordinates.Longitude));
    }
}

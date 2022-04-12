using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public class LocationService : ILocationService
{
    private readonly ICityProvider cityProvider;
    private readonly ILocationRepository locationRepository;

    public LocationService(ILocationRepository locationRepository, ICityProvider cityProvider)
    {
        this.locationRepository = Guards.ThrowIfNull(locationRepository);
        this.cityProvider = Guards.ThrowIfNull(cityProvider);
    }

    public async Task<Result<Location>> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        var maybeCity = await this.cityProvider.GetCityByCoordinatesAsync(coordinates);
        if (!maybeCity.TryGetValue(out var city))
        {
            return Result.Fail<Location>(LocationErrors.InvalidCoordinates(coordinates.Latitude, coordinates.Longitude));
        }

        var maybeLocation = await this.locationRepository.GetLocationByCityNameAsync(city.Name, cancellationToken).ConfigureAwait(false);
        if (!maybeLocation.TryGetValue(out var location))
        {
            return Result.Fail<Location>(LocationErrors.InvalidCity(city.Name));
        }

        return Result.Ok(location);
    }
}

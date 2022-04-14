using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public class LocationFactory : ILocationFactory
{
    private readonly ICoordinatesAgent coordinatesAgent;
    private readonly ICityRepository cityRepository;

    public LocationFactory(ICoordinatesAgent coordinatesAgent, ICityRepository cityRepository)
    {
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
        this.cityRepository = Guards.ThrowIfNull(cityRepository);
    }

    public async Task<Result<Location>> CreateAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        var maybeCityNameTask = this.coordinatesAgent.GetCityByCoordinatesAsync(coordinates, cancellationToken);
        var maybeLocationNameTask = this.coordinatesAgent.GetLocationByCoordinatesAsync(coordinates, cancellationToken);

        await Task.WhenAll(maybeCityNameTask, maybeLocationNameTask);

        var maybeCityName = await maybeCityNameTask;
        if (!maybeCityName.TryGetValue(out var cityName))
        {
            return CoordinatesErrors.CityNameNotRetrieved(coordinates);
        }

        var maybeLocationName = await maybeLocationNameTask;
        if (!maybeLocationName.TryGetValue(out var locationName))
        {
            return CoordinatesErrors.LocationNameNotRetrieved(coordinates);
        }

        var maybeCity = await this.cityRepository.GetCityByNameAsync(cityName, cancellationToken);
        if (!maybeCity.TryGetValue(out var city))
        {
            return CityErrors.CityNotFoundByName(cityName);
        }

        return new Location(Guid.NewGuid(), locationName, city, coordinates);
    }
}

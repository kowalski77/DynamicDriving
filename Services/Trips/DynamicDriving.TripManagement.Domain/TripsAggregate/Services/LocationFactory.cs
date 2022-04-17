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
        if (maybeCityName.HasNoValue)
        {
            return CoordinatesErrors.CityNameNotRetrieved(coordinates);
        }

        var maybeLocationName = await maybeLocationNameTask;
        if (maybeLocationName.HasNoValue)
        {
            return CoordinatesErrors.LocationNameNotRetrieved(coordinates);
        }

        var maybeCity = await this.cityRepository.GetCityByNameAsync(maybeCityName.Value, cancellationToken);
        if (maybeCity.HasNoValue)
        {
            return CityErrors.CityNotFoundByName(maybeCityName.Value);
        }

        return new Location(Guid.NewGuid(), maybeLocationName.Value, maybeCity.Value, coordinates);
    }
}

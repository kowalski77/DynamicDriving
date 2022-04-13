using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
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
        var maybeCityName = await this.coordinatesAgent.GetCityByCoordinatesAsync(coordinates, cancellationToken);
        if (!maybeCityName.TryGetValue(out var cityName))
        {
            return Result.Fail<Location>(new ErrorResult("", ""));
        }

        var maybeCity = await this.cityRepository.GetCityByName(cityName, cancellationToken);
        if (!maybeCity.TryGetValue(out var city))
        {
            return Result.Fail<Location>(new ErrorResult("", ""));
        }

        var maybeLocationName = await this.coordinatesAgent.GetLocationByCoordinatesAsync(coordinates, cancellationToken);
        if (!maybeLocationName.TryGetValue(out var locationName))
        {
            return Result.Fail<Location>(new ErrorResult("", ""));
        }

        var location = new Location(Guid.NewGuid(), locationName, city, coordinates);

        return Result.Ok(location);
    }
}

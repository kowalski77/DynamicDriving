using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate.Services;

public class CityValidator : ICityValidator
{
    private readonly ICoordinatesAgent coordinatesAgent;
    private readonly ICityRepository cityRepository;

    public CityValidator(ICityRepository cityRepository, ICoordinatesAgent coordinatesAgent)
    {
        this.cityRepository = Guards.ThrowIfNull(cityRepository);
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
    }

    public async Task<Result> ValidateCityCoordinates(Coordinates coordinates, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        var cityResult = await this.coordinatesAgent.GetCityByCoordinatesAsync(coordinates, cancellationToken);
        if (cityResult.Failure)
        {
            return cityResult;
        }

        var maybeCityEntity = await this.cityRepository.GetCityByName(cityResult.Value.Name, cancellationToken).ConfigureAwait(false);
        if (!maybeCityEntity.TryGetValue(out _))
        {
            return Result.Fail<City>(CityErrors.InvalidCity(cityResult.Value.Name));
        }

        return Result.Ok();
    }
}

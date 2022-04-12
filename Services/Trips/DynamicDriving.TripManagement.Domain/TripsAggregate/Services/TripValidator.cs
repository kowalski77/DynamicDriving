using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public class TripValidator : ITripValidator
{
    private readonly ICityRepository cityRepository;
    private readonly ICoordinatesAgent coordinatesAgent;

    public TripValidator(ICityRepository cityRepository, ICoordinatesAgent coordinatesAgent)
    {
        this.cityRepository = Guards.ThrowIfNull(cityRepository);
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
    }

    public async Task<Result> ValidateTripCoordinatesAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default)
    {
        var originCityResultTask = this.coordinatesAgent.GetCityByCoordinatesAsync(origin, cancellationToken);
        var destinationCityResultTask = this.coordinatesAgent.GetCityByCoordinatesAsync(origin, cancellationToken);

        await Task.WhenAll(originCityResultTask, destinationCityResultTask);

        var originCityResult = await originCityResultTask;
        if (originCityResult.Failure)
        {
            return originCityResult;
        }

        var destinationCityResult = await destinationCityResultTask;
        if (destinationCityResult.Failure)
        {
            return destinationCityResult;
        }

        var originValidationResult = await this.ValidateCityAsync(originCityResult.Value.Name, cancellationToken);
        if (originValidationResult.Failure)
        {
            return originValidationResult;
        }

        var destinationValidationResult = await this.ValidateCityAsync(destinationCityResult.Value.Name, cancellationToken);
        if (destinationValidationResult.Failure)
        {
            return destinationValidationResult;
        }

        return Result.Ok();
    }

    private async Task<Result> ValidateCityAsync(string cityName, CancellationToken cancellationToken = default)
    {
        var maybeCityEntity = await this.cityRepository.GetCityByName(cityName, cancellationToken).ConfigureAwait(false);

        return !maybeCityEntity.TryGetValue(out _) ? 
            Result.Fail<City>(CityErrors.InvalidCity(cityName)) : 
            Result.Ok();
    }
}

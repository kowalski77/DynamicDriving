using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ICoordinatesAgent coordinatesAgent;
    private readonly ICityRepository cityRepository;

    public TripService(ICoordinatesAgent coordinatesAgent, ICityRepository cityRepository)
    {
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
        this.cityRepository = Guards.ThrowIfNull(cityRepository);
    }

    public async Task<Result<Trip>> CreateDraftTripAsync(
        Guid id,
        UserId userId, DateTime pickUp, 
        Coordinates origin, Coordinates destination,
        CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(userId);
        Guards.ThrowIfNull(origin);
        Guards.ThrowIfNull(destination);

        // 1
        var (citiesResult, originCity, destinationCity) = await this.GetCitiesAsync(origin, destination, cancellationToken);
        if (citiesResult.Failure)
        {
            return Result.Fail<Trip>(citiesResult.Error!);
        }

        // 2
        var maybeOriginCityEntity = await this.cityRepository.GetCityByName(originCity, cancellationToken);
        if (!maybeOriginCityEntity.TryGetValue(out var originCityEntity))
        {
            return Result.Fail<Trip>(new ErrorResult("", ""));
        }
        var maybeDestinationCityEntity = await this.cityRepository.GetCityByName(destinationCity, cancellationToken);
        if (!maybeDestinationCityEntity.TryGetValue(out var destinationCityEntity))
        {
            return Result.Fail<Trip>(new ErrorResult("", ""));
        }

        // 3
        var (resultLocation, originLocation, destinationLocation) = await this.GetLocationsAsync(origin, destination, cancellationToken);
        if (resultLocation.Failure)
        {
            return Result.Fail<Trip>(resultLocation.Error!);
        }

        //4
        var originLocationEntity = new Location(Guid.NewGuid(), originLocation, originCityEntity, origin);
        var destinationLocationEntity = new Location(Guid.NewGuid(), destinationLocation, destinationCityEntity, destination);

        //5
        var trip = new Trip(id, userId, pickUp, originLocationEntity, destinationLocationEntity);

        return Result.Ok(trip);
    }

    private async Task<(Result, string, string)> GetCitiesAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var originCityResultTask = this.coordinatesAgent.GetCityByCoordinatesAsync(origin, cancellationToken);
        var destinationCityResultTask = this.coordinatesAgent.GetCityByCoordinatesAsync(destination, cancellationToken);

        await Task.WhenAll(originCityResultTask, destinationCityResultTask);

        var result = Result.Ok();

        var originCityResult = await originCityResultTask;
        if (!originCityResult.TryGetValue(out var originCity))
        {
            result = Result.Fail(new ErrorResult("", ""));
        }

        var destinationCityResult = await destinationCityResultTask;
        if (!destinationCityResult.TryGetValue(out var destinationCity))
        {
            result = Result.Fail(new ErrorResult("", ""));
        }

        return (result, originCity, destinationCity);
    }

    private async Task<(Result, string, string)> GetLocationsAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var originLocationResultTask = this.coordinatesAgent.GetLocationByCoordinatesAsync(origin, cancellationToken);
        var destinationLocationResultTask = this.coordinatesAgent.GetLocationByCoordinatesAsync(destination, cancellationToken);

        await Task.WhenAll(originLocationResultTask, destinationLocationResultTask);

        var result = Result.Ok();

        var originLocationResult = await originLocationResultTask;
        if (!originLocationResult.TryGetValue(out var originLocation))
        {
            result = Result.Fail(new ErrorResult("", ""));
        }

        var destinationLocationResult = await destinationLocationResultTask;
        if (!destinationLocationResult.TryGetValue(out var destinationLocation))
        {
            result = Result.Fail(new ErrorResult("", ""));
        }

        return (result, originLocation, destinationLocation);
    }
}

using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate.Services;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ICityValidator cityValidator;
    private readonly ICoordinatesAgent coordinatesAgent;

    public TripService(ICityValidator cityValidator, ICoordinatesAgent coordinatesAgent)
    {
        this.cityValidator = Guards.ThrowIfNull(cityValidator);
        this.coordinatesAgent = Guards.ThrowIfNull(coordinatesAgent);
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

        var validationResult = await this.ValidateCoordinatesAsync(origin, destination, cancellationToken);
        if (validationResult.Failure)
        {
            return Result.Fail<Trip>(validationResult.Error!);
        }

        var (result, originLocation, destinationLocation) = await this.GetLocationsAsync(origin, destination, cancellationToken);
        if (result.Failure)
        {
            return Result.Fail<Trip>(result.Error!);
        }

        var trip = new Trip(id, userId, pickUp, originLocation, destinationLocation);

        return Result.Ok(trip);
    }

    private async Task<Result> ValidateCoordinatesAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var originCityResultTask = this.cityValidator.ValidateCityCoordinates(origin, cancellationToken);
        var destinationCityResultTask = this.cityValidator.ValidateCityCoordinates(destination, cancellationToken);

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

        return Result.Ok();
    }

    private async Task<(Result, Location, Location)> GetLocationsAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var originLocationResultTask = this.coordinatesAgent.GetLocationByCoordinatesAsync(origin, cancellationToken);
        var destinationLocationResultTask = this.coordinatesAgent.GetLocationByCoordinatesAsync(destination, cancellationToken);

        await Task.WhenAll(originLocationResultTask, destinationLocationResultTask);
        var originLocationResult = await originLocationResultTask;

        var result = Result.Ok();
        if (originLocationResult.Failure)
        {
            result = originLocationResult;
        }

        var destinationLocationResult = await destinationLocationResultTask;
        if (destinationLocationResult.Failure)
        {
            result = destinationLocationResult;
        }

        return (result, originLocationResult.Value, destinationLocationResult.Value);
    }
}

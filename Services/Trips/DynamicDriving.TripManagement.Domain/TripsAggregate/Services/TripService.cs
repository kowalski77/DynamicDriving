using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ILocationFactory locationFactory;
    private readonly ITripValidator tripValidator;

    public TripService(ILocationFactory locationFactory, ITripValidator tripValidator)
    {
        this.locationFactory = Guards.ThrowIfNull(locationFactory);
        this.tripValidator = Guards.ThrowIfNull(tripValidator);
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

        var distanceValidator = await this.tripValidator.ValidateTripDistanceAsync(origin, destination, cancellationToken);
        if (distanceValidator.Failure)
        {
            return distanceValidator.Error!;
        }

        var (result, originLocation, destinationLocation) = await this.CreateLocationsAsync(origin, destination, cancellationToken);
        if (result.Failure)
        {
            return result.Error!;
        }

        return new Trip(id, userId, pickUp, originLocation, destinationLocation);
    }

    private async Task<(Result, Location, Location)> CreateLocationsAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default)
    {
        var originLocation = await this.locationFactory.CreateAsync(origin, cancellationToken);

        Result result = Result.Ok();
        if (originLocation.Failure)
        {
            result= Result.Fail<Trip>(originLocation.Error!);
        }
        var destinationLocation = await this.locationFactory.CreateAsync(destination, cancellationToken);
        if (destinationLocation.Failure)
        {
            result =  Result.Fail<Trip>(destinationLocation.Error!);
        }

        return (result, originLocation.Value, destinationLocation.Value);
    }
}

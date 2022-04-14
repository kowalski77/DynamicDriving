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
            return Result.Fail<Trip>(distanceValidator.Error!);
        }

        var originLocation = await this.locationFactory.CreateAsync(origin, cancellationToken);
        if (originLocation.Failure)
        {
            return Result.Fail<Trip>(originLocation.Error!);
        }
        var destinationLocation = await this.locationFactory.CreateAsync(destination, cancellationToken);
        if (destinationLocation.Failure)
        {
            return Result.Fail<Trip>(destinationLocation.Error!);
        }

        var trip = new Trip(id, userId, pickUp, originLocation.Value, destinationLocation.Value);

        return Result.Ok(trip);
    }
}

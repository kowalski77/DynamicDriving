using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ILocationFactory locationFactory;

    public TripService(ILocationFactory locationFactory)
    {
        this.locationFactory = Guards.ThrowIfNull(locationFactory);
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

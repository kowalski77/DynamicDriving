using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ILocationService locationService;

    public TripService(ILocationService locationService)
    {
        this.locationService = Guards.ThrowIfNull(locationService);
    }

    public async Task<Result<Trip>> CreateDraftTripAsync(
        Guid id,
        UserId userId, DateTime pickUp, 
        Coordinates origin, Coordinates destination,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(origin);
        ArgumentNullException.ThrowIfNull(destination);

        var originLocationResult = await this.locationService.ValidateAsync(origin, cancellationToken);
        if (originLocationResult.Failure)
        {
            return Result.Fail<Trip>(originLocationResult.Error!);
        }

        var destinationLocationResult = await this.locationService.ValidateAsync(destination, cancellationToken);
        if (destinationLocationResult.Failure)
        {
            return Result.Fail<Trip>(destinationLocationResult.Error!);
        }

        var trip = new Trip(id, userId, pickUp, originLocationResult.Value, destinationLocationResult.Value);

        return Result.Ok(trip);
    }
}

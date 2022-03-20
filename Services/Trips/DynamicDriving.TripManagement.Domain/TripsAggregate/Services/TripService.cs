using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public sealed class TripService : ITripService
{
    private readonly ICoordinatesValidator coordinatesValidator;

    public TripService(ICoordinatesValidator coordinatesValidator)
    {
        this.coordinatesValidator = Guards.ThrowIfNull(coordinatesValidator);
    }

    public async Task<Result<Trip>> CreateDraftTripAsync(
        User user, DateTime pickUp, 
        Coordinates origin, Coordinates destination,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(origin);
        ArgumentNullException.ThrowIfNull(destination);

        var originValidation = await this.coordinatesValidator.ValidateAsync(origin, cancellationToken).ConfigureAwait(false);
        if (originValidation.Failure)
        {
            return Result.Fail<Trip>(originValidation.Error!);
        }

        var destinationValidation = await this.coordinatesValidator.ValidateAsync(destination, cancellationToken).ConfigureAwait(false);
        if (destinationValidation.Failure)
        {
            return Result.Fail<Trip>(destinationValidation.Error!);
        }

        var trip = new Trip(user, pickUp, origin, destination);

        return Result.Ok(trip);
    }
}

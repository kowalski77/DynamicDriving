using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CarsAggregate;
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
        User user, Car car, DateTime pickUp, 
        Coordinates origin, Coordinates destination)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(car);
        ArgumentNullException.ThrowIfNull(origin);
        ArgumentNullException.ThrowIfNull(destination);

        var originValidation = await this.coordinatesValidator.ValidateAsync(origin).ConfigureAwait(false);
        if (originValidation.Failure)
        {
            return Result.Fail<Trip>(originValidation.Error!);
        }

        var destinationValidation = await this.coordinatesValidator.ValidateAsync(destination).ConfigureAwait(false);
        if (destinationValidation.Failure)
        {
            return Result.Fail<Trip>(destinationValidation.Error!);
        }

        var trip = new Trip(user, car, pickUp, origin, destination);

        return Result.Ok(trip);
    }
}

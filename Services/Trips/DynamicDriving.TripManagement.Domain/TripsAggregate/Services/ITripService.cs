using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CarsAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ITripService
{
    Task<Result<Trip>> CreateDraftTripAsync(User user, Car car, DateTime pickUp, Coordinates origin, Coordinates destination);
}

using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ITripService
{
    Task<Result<Trip>> CreateDraftTripAsync(Guid id, User user, DateTime pickUp, Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default);
}

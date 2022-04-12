using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ITripService
{
    Task<Result<Trip>> CreateDraftTripAsync(Guid id, UserId userId, DateTime pickUp, Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default);
}

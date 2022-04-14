using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ITripValidator
{
    Task<Result> ValidateTripDistanceAsync(Coordinates origin, Coordinates destination, CancellationToken cancellationToken = default);
}

using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

public interface ILocationFactory
{
    Task<Result<Location>> CreateAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
}

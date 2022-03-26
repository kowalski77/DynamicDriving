using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ILocationService
{
     Task<Result<Location>> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
}

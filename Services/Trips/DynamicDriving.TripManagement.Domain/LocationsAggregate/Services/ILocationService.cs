using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ILocationService
{
     Task<Result<Location>> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
}

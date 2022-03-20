using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ICoordinatesValidator
{
     Task<Result> ValidateAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
}

using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate.Services;

public interface ICityValidator
{
     Task<Result> ValidateCityCoordinates(Coordinates coordinates, CancellationToken cancellationToken = default);
}

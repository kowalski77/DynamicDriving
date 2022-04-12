using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Domain.Common;

public interface ICoordinatesAgent
{
    Task<Result<City>> GetCityByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default);

    Task<Result<Location>> GetLocationByCoordinatesAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
}

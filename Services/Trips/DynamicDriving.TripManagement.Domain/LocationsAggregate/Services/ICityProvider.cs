using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ICityProvider
{
    Task<Maybe<City>> GetCityByCoordinatesAsync(Coordinates coordinates);
}

using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;

public interface ICityProvider
{
    Task<Maybe<City>> GetCityByCoordinatesAsync(Coordinates coordinates);
}

using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate;

public interface ICityRepository
{
    Task<Maybe<City>> GetCityByName(string name, CancellationToken cancellationToken = default);
}

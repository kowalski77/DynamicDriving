using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate;

public interface ICityRepository
{
    Task<Maybe<City>> GetCityByNameAsync(string name, CancellationToken cancellationToken = default);
}

using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class CityRepository : ICityRepository
{
    public Task<Maybe<City>> GetCityByName(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

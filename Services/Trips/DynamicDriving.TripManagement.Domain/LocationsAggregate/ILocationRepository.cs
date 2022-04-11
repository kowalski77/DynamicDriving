using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public interface ILocationRepository : IRepository<Location>
{
    Task<Maybe<Location>> GetLocationByCityNameAsync(string city, CancellationToken cancellationToken = default);
}

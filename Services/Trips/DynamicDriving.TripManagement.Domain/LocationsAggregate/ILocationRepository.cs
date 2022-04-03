using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public interface ILocationRepository : IRepository<Location>
{
    public Task<IReadOnlyList<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
}

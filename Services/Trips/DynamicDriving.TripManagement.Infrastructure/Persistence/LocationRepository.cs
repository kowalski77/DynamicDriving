using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(TransactionContext context) : base(context)
    {
    }

    public Task<IReadOnlyList<Location>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

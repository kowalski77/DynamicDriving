using DynamicDriving.TripManagement.Domain.LocationsAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class LocationRepository : ILocationRepository
{
    public Task<IReadOnlyList<Location>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public interface ILocationRepository
{
    public Task<IReadOnlyList<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);
}

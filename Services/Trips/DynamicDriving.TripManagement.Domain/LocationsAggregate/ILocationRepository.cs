namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public interface ILocationRepository
{
    public IReadOnlyList<Location> GetLocations();
}

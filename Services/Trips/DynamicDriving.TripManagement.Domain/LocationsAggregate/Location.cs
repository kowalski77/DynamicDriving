using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public class Location : Entity, IAggregateRoot
{
    public Location(string name, Coordinates coordinates)
    {
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.Coordinates = Guards.ThrowIfNull(coordinates);
    }

    public string Name { get; private set; }

    public Coordinates Coordinates { get; private set; }
}

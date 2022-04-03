#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public sealed class Location : Entity, IAggregateRoot
{
    private Location() { }

    public Location(string name, string city, Coordinates coordinates)
    {
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.City = Guards.ThrowIfNullOrEmpty(city);
        this.Coordinates = Guards.ThrowIfNull(coordinates);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public string City { get; private set; }

    public Coordinates Coordinates { get; private set; }

    public bool IsPermittedArea(Location otherLocation)
    {
        ArgumentNullException.ThrowIfNull(otherLocation);

        return this.City == otherLocation.City;
    }
}

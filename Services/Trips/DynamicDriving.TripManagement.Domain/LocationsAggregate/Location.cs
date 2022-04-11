#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public sealed class Location : Entity, IAggregateRoot
{
    private Location() { }

    public Location(string name, City city, Coordinates coordinates)
    {
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.City = Guards.ThrowIfNull(city);
        this.Coordinates = Guards.ThrowIfNull(coordinates);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public City City { get; private set; }

    public Coordinates Coordinates { get; private set; }
}

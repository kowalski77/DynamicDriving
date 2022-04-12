#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public sealed class Location : Entity
{
    private Location() { }

    public Location(Guid id, string name, City city, Coordinates coordinates)
    {
        this.Id = Guards.ThrowIfEmpty(id);
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.City = Guards.ThrowIfNull(city);
        this.Coordinates = Guards.ThrowIfNull(coordinates);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public City City { get; private set; }

    public Coordinates Coordinates { get; private set; }
}

#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public sealed class Trip : Entity, IAggregateRoot
{
    private Trip() { }

    public Trip(Guid id, UserId userId, DateTime pickUp, Location origin, Location destination)
    {
        this.Id = Guards.ThrowIfEmpty(id);
        this.UserId = Guards.ThrowIfNull(userId);
        this.PickUp = pickUp;
        this.Origin = Guards.ThrowIfNull(origin);
        this.Destination = Guards.ThrowIfNull(destination);
        this.TripStatus = TripStatus.Draft;
        this.Kilometers = 0;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public UserId UserId { get; private set; }

    public Driver? Driver { get; private set; }

    public DateTime PickUp { get; private set; }

    public Location Origin { get; private set; }

    public Location Destination { get; private set; }

    public TripStatus TripStatus { get; private set; }

    public decimal Kilometers { get; private set; }
}

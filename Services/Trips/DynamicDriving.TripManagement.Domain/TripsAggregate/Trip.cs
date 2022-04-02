#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public sealed class Trip : Entity, IAggregateRoot
{
    private Trip() { }

    public Trip(User user, DateTime pickUp, Location origin, Location destination)
    {
        this.User = Guards.ThrowIfNull(user);
        this.PickUp = pickUp;
        this.Origin = Guards.ThrowIfNull(origin);
        this.Destination = Guards.ThrowIfNull(destination);
        this.TripStatus = TripStatus.Draft;
        this.Kilometers = 0;
    }

    public User User { get; private set; }

    public Driver? Driver { get; private set; }

    public DateTime PickUp { get; private set; }

    public Location Origin { get; private set; }

    public Location Destination { get; private set; }

    public TripStatus TripStatus { get; private set; }

    public decimal Kilometers { get; private set; }
}

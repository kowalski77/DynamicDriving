#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.CarsAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public class Trip : Entity, IAggregateRoot
{
    private Trip() { }

    public Trip(User user, DateTime pickUp, Coordinates origin, Coordinates destination)
    {
        this.User = Guards.ThrowIfNull(user);
        this.PickUp = pickUp;
        this.Origin = Guards.ThrowIfNull(origin);
        this.Destination = Guards.ThrowIfNull(destination);
        this.TripStatus = TripStatus.Draft;
        this.Kilometers = 0;
    }

    public User User { get; private set; }

    public Car? Car { get; private set; }

    public DateTime PickUp { get; private set; }

    public Coordinates Origin { get; private set; }

    public Coordinates Destination { get; private set; }

    public TripStatus TripStatus { get; private set; }

    public decimal Kilometers { get; private set; }
}

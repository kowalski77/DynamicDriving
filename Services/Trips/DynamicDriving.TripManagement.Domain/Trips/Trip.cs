#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.Cars;
using DynamicDriving.TripManagement.Domain.Users;

namespace DynamicDriving.TripManagement.Domain.Trips;

public class Trip : Entity, IAggregateRoot
{
    private Trip() { }

    public Trip(Guid id, User user, Car car, DateTime pickUp, Coordinates coordinates)
    {
        this.Id = Guards.ThrowIfEmpty(id);
        this.User = Guards.ThrowIfNull(user);
        this.Car = Guards.ThrowIfNull(car);
        this.PickUp = pickUp;
        this.Coordinates = Guards.ThrowIfNull(coordinates);
        this.TripStatus = TripStatus.Draft;
    }

    public User User { get; set; }

    public Car Car { get; set; }

    public DateTime PickUp { get; set; }

    public Coordinates Coordinates { get; set; }

    public TripStatus TripStatus { get; set; }
}

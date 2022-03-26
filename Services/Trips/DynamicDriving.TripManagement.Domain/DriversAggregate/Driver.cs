using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.DriversAggregate;

public class Driver : Entity, IAggregateRoot
{
    public Driver(string name, string description, Car car)
    {
        this.Name = name;
        this.Description = description;
        this.Car = car;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public Car Car { get; private set; }
}

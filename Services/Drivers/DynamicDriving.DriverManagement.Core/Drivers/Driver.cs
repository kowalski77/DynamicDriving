using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.DriverManagement.Core.Drivers;

public class Driver : IEntity
{
    public Driver(Guid id, string name, Car car, bool isAvailable)
    {
        this.Id = id;
        this.Name = name;
        this.Car = car;
        this.IsAvailable = isAvailable;
    }

    public Guid Id { get; private set;}

    public string Name { get; private set;}

    public Car Car { get;  private set;}

    public bool IsAvailable { get; private set;}
}

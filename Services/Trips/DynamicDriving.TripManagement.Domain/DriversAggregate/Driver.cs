#pragma warning disable 8618
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.DriversAggregate;

public sealed class Driver : Entity, IAggregateRoot
{
    public Driver() { }

    public Driver(string name, string description, Car car, Coordinates currentCoordinates)
    {
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.Description = description;
        this.Car = Guards.ThrowIfNull(car);
        this.CurrentCoordinates = Guards.ThrowIfNull(currentCoordinates);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public Car Car { get; private set; }

    public Coordinates CurrentCoordinates { get; private set; }
}

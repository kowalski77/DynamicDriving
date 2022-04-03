#pragma warning disable 8618
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.DriversAggregate;

public sealed class Car : Entity
{
    private Car() { }

    public Car(string brand, string model, CarType carType)
    {
        this.Brand = brand;
        this.Model = model;
        this.CarType = carType;
    }

    public int Id { get; private set; }

    public string Brand { get; private set; }

    public string Model { get; private set; }

    public CarType CarType { get; private set; }
}

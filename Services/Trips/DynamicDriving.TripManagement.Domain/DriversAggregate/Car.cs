using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.DriversAggregate;

public class Car : Entity
{
    public Car(string brand, string model, CarType carType)
    {
        this.Brand = brand;
        this.Model = model;
        this.CarType = carType;
    }

    public string Brand { get; private set; }

    public string Model { get; private set; }

    public CarType CarType { get; private set; }
}

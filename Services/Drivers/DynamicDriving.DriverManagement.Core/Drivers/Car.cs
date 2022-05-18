namespace DynamicDriving.DriverManagement.Core.Drivers;

public class Car
{
    public Car(Guid id, string model, CarType carType)
    {
        this.Id = id;
        this.Model = model;
        this.CarType = carType;
    }

    public Guid Id { get; }

    public string Model { get; }

    public CarType CarType { get; }
}

namespace DynamicDriving.Models;

public record RegisterDriverRequest(Guid Id, string Name, Car Car, bool IsAvailable);

public record RegisterDriverResponse(Guid DriverId);

public record Car(Guid Id, string Model, CarType CarType);

public enum CarType
{
    None,
    Sedan,
    Coupe,
    SportsCar,
    StationWagon,
    HatchBack,
    Convertible,
    Suv,
    Minivan,
    Pickup
}

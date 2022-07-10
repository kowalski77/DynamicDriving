namespace DynamicDriving.Contracts.Drivers;

public record RegisterDriverRequest(Guid Id, string Name, Car Car, bool IsAvailable);

public record RegisterDriverResponse(Guid DriverId);

public record AssignDriverRequest(Guid TripId);

public record AssignDriverResponse(Guid TripId, Guid DriverId);

public record DriversSummaryResponse(IEnumerable<DriverSummary> Drivers);

public record DriverSummary(Guid Id, string Name, string Car, bool IsAvailable);

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

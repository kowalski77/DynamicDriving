namespace DynamicDriving.Events;

public sealed record DriverCreated(Guid DriverId, string Name, string CarName, string CarDescription);

public sealed record DriverAssigned(Guid TripId, Guid DriverId);

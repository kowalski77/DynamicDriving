namespace DynamicDriving.Events;

public sealed record DriverAssignedToTrip(Guid Id, Guid TripId, Driver Driver) : IIntegrationEvent;

public sealed record Driver(Guid Id, string Name, string Car);

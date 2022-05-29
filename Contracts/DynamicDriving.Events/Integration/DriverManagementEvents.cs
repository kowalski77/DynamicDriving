namespace DynamicDriving.Events;

public sealed record DriverAssigned(Guid Id, Guid TripId, Driver Driver) : IIntegrationEvent;

public sealed record Driver(Guid Id, string Name, string Car);

namespace DynamicDriving.Events;

public sealed record DriverCreated(Guid Id, Guid DriverId, string Name, string CarName, string CarDescription) : IIntegrationEvent;

public sealed record DriverAssigned(Guid Id, Guid TripId, Guid DriverId) : IIntegrationEvent;

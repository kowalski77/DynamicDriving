namespace DynamicDriving.Events;

public sealed record TripConfirmed(Guid Id, Guid TripId) : IIntegrationEvent;

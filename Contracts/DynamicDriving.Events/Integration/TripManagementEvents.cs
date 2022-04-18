namespace DynamicDriving.Events;

public sealed record TripConfirmed(Guid Id, Guid TripId, DateTime PickUp, decimal Latitude, decimal Longitude) : IIntegrationEvent;

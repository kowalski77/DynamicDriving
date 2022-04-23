namespace DynamicDriving.Events;

public sealed record TripConfirmed(Guid Id, Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : IIntegrationEvent;

namespace DynamicDriving.Contracts.Events;

public sealed record TripCreated(Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude);

public sealed record TripConfirmed(Guid CorrelationId);

public sealed record TripInvalidated(Guid TripId, Guid CorrelationId);

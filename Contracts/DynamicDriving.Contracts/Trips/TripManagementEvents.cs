namespace DynamicDriving.Contracts.Trips;

public sealed record TripCreated(Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude, int Credits);

public sealed record TripConfirmed(Guid CorrelationId);

public sealed record TripInvalidated(Guid TripId, Guid CorrelationId);

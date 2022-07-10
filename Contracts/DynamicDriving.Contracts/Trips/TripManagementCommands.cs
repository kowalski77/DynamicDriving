namespace DynamicDriving.Contracts.Trips;

public record ConfirmTrip(Guid TripId, Guid CorrelationId);

public record InvalidateTrip(Guid TripId, Guid CorrelationId);


namespace DynamicDriving.Contracts.Commands;

public record ConfirmTrip(Guid TripId, Guid CorrelationId);

public record InvalidateTrip(Guid TripId, Guid CorrelationId);


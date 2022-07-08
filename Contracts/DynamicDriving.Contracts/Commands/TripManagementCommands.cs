namespace DynamicDriving.Contracts.Commands;

public record ConfirmTrip(Guid TripId, Guid CorrelationId);

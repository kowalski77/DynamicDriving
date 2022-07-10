namespace DynamicDriving.Contracts.TripService;

public record ConfirmTripRequested(Guid UserId, Guid TripId, int Credits, Guid CorrelationId);
namespace DynamicDriving.Contracts.TripService;

public record BookingRequested(Guid UserId, Guid TripId, int Credits, Guid CorrelationId);
namespace DynamicDriving.Contracts.TripService;

public record BookingRequested(Guid UserId, Guid TripId, int Credits, Guid CorrelationId);

public record GetBookingState(Guid CorrelationId);
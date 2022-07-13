using System.ComponentModel.DataAnnotations;

namespace DynamicDriving.Contracts.TripService;

public record SubmitBookingRequest([Required] Guid? TripId, [Required] Guid? IdempotencyId);

public record BookingResponse(Guid UserId, Guid TripId, int Credits, string State, string Reason, DateTimeOffset Received, DateTimeOffset LastUpdated);

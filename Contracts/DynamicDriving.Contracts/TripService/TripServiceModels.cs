using System.ComponentModel.DataAnnotations;

namespace DynamicDriving.Contracts.TripService;

public record SubmitBookingRequest([Required] Guid? TripId, [Range(1, 100)] int Credits);

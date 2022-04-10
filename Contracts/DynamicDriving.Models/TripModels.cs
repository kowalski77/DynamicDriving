[assembly: CLSCompliant(false)]

namespace DynamicDriving.Models;

public sealed record CreateDraftTripRequest(Guid TripId, Guid UserId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude);

public sealed record CreateDraftTripResponse(Guid TripId);

public record TripByIdResponse(string UserName, string? DriverName, DateTime PickUpTime, TripByIdLocationResponse Origin, TripByIdLocationResponse Destination);

public record TripByIdLocationResponse(string Name, string City, decimal Longitude, decimal Latitude);

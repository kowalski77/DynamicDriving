﻿[assembly: CLSCompliant(false)]

namespace DynamicDriving.Contracts.Trips;

public sealed record CreateDraftTripRequest(DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude);

public sealed record CreateDraftTripResponse(Guid TripId);

public record TripByIdResponse(Guid UserId, string? DriverName, DateTime PickUpTime, TripByIdLocationResponse Origin, TripByIdLocationResponse Destination, int Credits);

public record TripsByUserResponse(Guid UserId, IEnumerable<TripSummary> TripSummaries);

public record TripSummary(string? DriverName, string Status, string Origin, string Destination);

public record TripByIdLocationResponse(string Name, string City, decimal Longitude, decimal Latitude);

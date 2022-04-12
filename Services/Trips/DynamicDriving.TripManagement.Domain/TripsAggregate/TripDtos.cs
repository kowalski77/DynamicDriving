namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public record DraftTripDto(Guid Id);

public record TripByIdDto(Guid UserId, string? DriverName, DateTime PickUpTime, TripByIdLocationDto Origin, TripByIdLocationDto Destination);

public record TripByIdLocationDto(string Name, string City, decimal Longitude, decimal Latitude);

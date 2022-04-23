using DynamicDriving.DriverManagement.Core.Drivers;

namespace DynamicDriving.DriverManagement.Core.Trips;

public record Trip(Guid Id, DateTime PickUp, Coordinates Origin, Coordinates Destination)
{
    public TripStatus TripStatus { get; init; } = TripStatus.Unassigned;

    public Driver? Driver { get; init; }
}

public record Coordinates(decimal Latitude, decimal Longitude);

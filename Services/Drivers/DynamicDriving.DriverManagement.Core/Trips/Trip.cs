using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.DriverManagement.Core.Trips;

public record Trip(Guid Id, DateTime PickUp, Coordinates Origin, Coordinates Destination) : IEntity
{
    public TripStatus TripStatus { get; init; } = TripStatus.Unassigned;

    public Driver? Driver { get; init; }
}

public record Coordinates(decimal Latitude, decimal Longitude);

public enum TripStatus
{
    Unassigned,
    Assigned,
    ToOrigin,
    ToDestination,
    Canceled,
    Finished
}

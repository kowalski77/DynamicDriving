namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public static class TripErrorConstants
{
    public const string DriverAssignFailedCode = "driver.not.assigned";

    public const string DriverAssignFailedMessage = "Driver could not be assigned with status trip {0}";

    public const string MinimumDistanceBetweenLocationsCode = "minimumdistance.between.locations";

    public const string MinimumDistanceBetweenLocationsMessage = "The minimum distance between locations is {0} km";
}

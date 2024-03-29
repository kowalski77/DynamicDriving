﻿namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public static class TripErrorConstants
{
    public const string DriverAssignFailedCode = "driver.not.assigned";
    public const string DriverAssignFailedMessage = "Driver could not be assigned with status trip {0}";

    public const string MinimumDistanceBetweenLocationsCode = "minimumdistance.between.locations";
    public const string MinimumDistanceBetweenLocationsMessage = "The minimum distance between locations is {0} km";

    public const string ConfirmFailedCode = "confirm.not.possible";
    public const string ConfirmFailedMessage = "Can not confirm due trip status is {0}";

    public const string InvalidateFailedCode = "invalidate.not.possible";
    public const string InvalidateFailedMessage = "Can not invalidate due trip status is {0}";

    public const string UserTripNotFoundCode = "usertrip.not.found";
    public const string TripNotFoundMessage = "Trip for user with id {0} not found";
}

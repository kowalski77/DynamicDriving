using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public static class TripErrors
{
    public static ErrorResult DriverAssignedFailed(TripStatus status)
    {
        return new ErrorResult(
            TripErrorConstants.DriverAssignFailedCode,
            string.Format(CultureInfo.InvariantCulture, TripErrorConstants.DriverAssignFailedMessage, status.ToString()));
    }

    public static ErrorResult MinimumDistanceBetweenLocations(decimal distance)
    {
        return new ErrorResult(
            TripErrorConstants.MinimumDistanceBetweenLocationsCode,
            string.Format(
                CultureInfo.InvariantCulture, 
                TripErrorConstants.MinimumDistanceBetweenLocationsMessage, 
                distance.ToString(CultureInfo.InvariantCulture)));
    }
}

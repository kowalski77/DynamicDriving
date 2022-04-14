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
}

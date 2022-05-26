using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.DriverManagement.Core.Trips;

public static class TripErrors
{
    public static ErrorResult NoDriverAvailable()
    {
        return new ErrorResult(TripErrorConstants.NoDriverAvailableCode, TripErrorConstants.NoDriverAvailableMessage);
    }
}

using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.Common;

public static class CoordinatesErrors
{
    public static ErrorResult OutOfRangeCoordinates(string argument, decimal min, decimal max)
    {
        return new ErrorResult(
            CoordinatesErrorConstants.OutOfRangeCoordinatesCode, 
            string.Format(CultureInfo.InvariantCulture, CoordinatesErrorConstants.OutOfRangeCoordinatesMessage, argument, min, max));
    }
}

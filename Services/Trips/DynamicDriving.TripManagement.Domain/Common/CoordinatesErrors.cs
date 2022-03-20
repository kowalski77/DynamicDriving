using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.Common;

public static class CoordinatesErrors
{
    public static ErrorResult OutOfRangeCoordinates(string argument, decimal min, decimal max)
    {
        return new ErrorResult(
            DomainErrorConstants.InvalidCoordinatesCode, 
            string.Format(CultureInfo.InvariantCulture, DomainErrorConstants.OutOfRangeCoordinatesMessage, argument, min, max));
    }
}

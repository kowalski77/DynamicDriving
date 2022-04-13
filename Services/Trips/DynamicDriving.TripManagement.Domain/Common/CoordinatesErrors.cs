using System.Globalization;
using DynamicDriving.SharedKernel;
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

    public static ErrorResult CityNameNotRetrieved(Coordinates coordinates)
    {
        Guards.ThrowIfNull(coordinates);

        return new ErrorResult(
            CoordinatesErrorConstants.CityNameCode,
            string.Format(CultureInfo.InvariantCulture, CoordinatesErrorConstants.CityNameMessage, coordinates.Latitude, coordinates.Longitude));
    }

    public static ErrorResult LocationNameNotRetrieved(Coordinates coordinates)
    {
        Guards.ThrowIfNull(coordinates);

        return new ErrorResult(
            CoordinatesErrorConstants.LocationNameCode,
            string.Format(CultureInfo.InvariantCulture, CoordinatesErrorConstants.LocationNameMessage, coordinates.Latitude, coordinates.Longitude));
    }
}

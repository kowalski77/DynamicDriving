using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public static class LocationErrors
{
    public static ErrorResult InvalidCoordinates(decimal latitude, decimal longitude)
    {
        return new(
            LocationErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, LocationErrorConstants.InvalidCoordinatesMessage, latitude, longitude));
    }

    public static ErrorResult InvalidCity(string name)
    {
        return new(
            LocationErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, LocationErrorConstants.InvalidCityMessage, name));
    }

    public static ErrorResult OutOfRangeCoordinates(string argument, decimal min, decimal max)
    {
        return new ErrorResult(
            LocationErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, LocationErrorConstants.OutOfRangeCoordinatesMessage, argument, min, max));
    }
}

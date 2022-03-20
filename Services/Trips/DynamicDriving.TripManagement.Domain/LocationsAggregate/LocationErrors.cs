using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public static class LocationErrors
{
    public static ErrorResult InvalidCoordinates(decimal latitude, decimal longitude)
    {
        return new(
            DomainErrorConstants.InvalidCoordinatesCode, 
            string.Format(CultureInfo.InvariantCulture, DomainErrorConstants.InvalidCoordinatesMessage, latitude, longitude));
    }

    public static ErrorResult InvalidCityCoordinates(decimal latitude, decimal longitude)
    {
        return new(
            DomainErrorConstants.InvalidCoordinatesCode, 
            string.Format(CultureInfo.InvariantCulture, DomainErrorConstants.InvalidCityCoordinatesMessage, latitude, longitude));
    }
}

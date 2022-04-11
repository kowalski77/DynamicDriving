using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public static class LocationErrors
{
    public static ErrorResult InvalidCoordinates(decimal latitude, decimal longitude)
    {
        return new(
            DomainErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, DomainErrorConstants.InvalidCoordinatesMessage, latitude, longitude));
    }

    public static ErrorResult InvalidCity(string name)
    {
        return new(
            DomainErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, DomainErrorConstants.InvalidCityMessage, name));
    }
}

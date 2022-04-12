using System.Globalization;
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate;

public static class CityErrors
{
    public static ErrorResult InvalidCity(string name)
    {
        return new(
            CityErrorConstants.InvalidCityCode, 
            string.Format(CultureInfo.InvariantCulture, CityErrorConstants.InvalidCityMessage, name));
    }
}

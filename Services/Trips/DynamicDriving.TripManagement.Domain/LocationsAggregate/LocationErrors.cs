using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.TripManagement.Domain.LocationsAggregate;

public static class LocationErrors
{
    public static ErrorResult InvalidCoordinates(decimal latitude, decimal longitude)
    {
        return new("invalid.coordinates", $"Invalid Coordinates, latitude: {latitude}, longitude: {longitude}");
    }

    public static ErrorResult InvalidCityCoordinates(decimal latitude, decimal longitude)
    {
        return new("invalid.city.coordinates", $"Coordinates, latitude: {latitude}, longitude: {longitude} do not belong to a valid city");
    }
}

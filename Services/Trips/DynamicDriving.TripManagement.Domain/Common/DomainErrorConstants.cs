namespace DynamicDriving.TripManagement.Domain.Common;

public static class DomainErrorConstants
{
    public const string InvalidCoordinatesCode = "coordinates.not.valid";

    public const string InvalidCoordinatesMessage = "Invalid Coordinates, latitude: {0}, longitude: {1}";

    public const string OutOfRangeCoordinatesMessage = "{0} should be between {1} and {2}";

    public const string InvalidCityCoordinatesMessage = "Coordinates, latitude: {0}, longitude: {1} do not belong to a valid city";

}

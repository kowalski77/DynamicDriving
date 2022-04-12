namespace DynamicDriving.TripManagement.Domain.Common;

public static class DomainErrorConstants
{
    public const string InvalidCityCode = "city.not.valid";

    public const string InvalidCoordinatesMessage = "Invalid Coordinates, latitude: {0}, longitude: {1}";

    public const string OutOfRangeCoordinatesMessage = "{0} should be between {1} and {2}";

    public const string InvalidCityMessage = "{0} do not belong to a valid city";
}

namespace DynamicDriving.DriverManagement.Core.Trips;

public class Coordinates
{
    private Coordinates(decimal latitude, decimal longitude)
    {
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public decimal Latitude { get; private set;}

    public decimal Longitude { get; private set;}

    public static Coordinates CreateInstance(decimal latitude, decimal longitude)
    {
        return new Coordinates(latitude, longitude);
    }
}

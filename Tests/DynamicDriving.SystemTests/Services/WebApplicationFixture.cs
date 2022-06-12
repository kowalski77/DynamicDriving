namespace DynamicDriving.SystemTests.Services;

public sealed class WebApplicationFixture : IDisposable
{
    public WebApplicationFixture()
    {
        this.Drivers = new DriversWebApplicationFactory();
        this.Trips = new TripsWebApplicationFactory();
    }

    public DriversWebApplicationFactory Drivers { get; }

    public TripsWebApplicationFactory Trips { get; }

    public void Dispose()
    {
        this.Drivers.Dispose();
        this.Trips.Dispose();
    }
}

using System.Net.Http.Json;
using AutoFixture;
using DynamicDriving.Models;

namespace DynamicDriving.SystemTests.Services;

public class IntegrationEventsScenarios
{
    private const string DriversEndpoint = "/api/v1/Drivers";
    private readonly IFixture fixture = new Fixture();

    [Fact]
    public async Task Driver_is_created_in_trip_management_when_is_registered()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var request = this.fixture.Build<RegisterDriverRequest>().With(x => x.Id, driverId).Create();

        await using var drivers = new DriversWebApplicationFactory();
        await using var trips = new TripsWebApplicationFactory();

        // Act
        var response = await drivers.HttpClient.PostAsJsonAsync(DriversEndpoint, request);

        // Assert

    }
}

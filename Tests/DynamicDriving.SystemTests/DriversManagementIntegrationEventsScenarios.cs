using System.Net.Http.Json;
using AutoFixture;
using DynamicDriving.Models;
using DynamicDriving.SystemTests.Services;
using DynamicDriving.SystemTests.Support;
using FluentAssertions;
using Xunit.Sdk;

namespace DynamicDriving.SystemTests;

public class DriversManagementIntegrationEventsScenarios : IClassFixture<WebApplicationFixture>
{
    private const string DriversEndpoint = "/api/v1/Drivers";
    private const string TripsEndpoint = "api/v1/Trips";

    private readonly IFixture fixture;
    private readonly WebApplicationFixture webApplicationFixture;

    public DriversManagementIntegrationEventsScenarios(WebApplicationFixture webApplicationFixture)
    {
        this.fixture = new Fixture();
        this.webApplicationFixture = webApplicationFixture;
    }

    [Fact]
    public async Task Driver_is_created_in_trip_management_when_is_registered_in_driver_management()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var request = this.fixture.Build<RegisterDriverRequest>().With(x => x.Id, driverId).Create();

        // Act
        var response = await this.webApplicationFixture.Drivers.HttpClient.PostAsJsonAsync(DriversEndpoint, request);

        // Assert
        response.EnsureSuccessStatusCode();

        await Retry.Handle<InvalidOperationException>(1000, 5)
            .ExecuteAsync(async () =>
            {
                var driver = await this.webApplicationFixture.Trips.GetDriverByIdAsync(driverId);
                driver.Name.Should().Be(request.Name);
            });
    }

    [Fact]
    public async Task Driver_is_assigned_in_trip_management_when_is_assigned_in_driver_management()
    {
        // Arrange
        var tripId = Guid.Parse(SystemTestsConstants.TripId);
        var request = this.fixture.Build<AssignDriverRequest>().With(x => x.TripId, tripId).Create();

        // Act
        var response = await this.webApplicationFixture.Drivers.HttpClient.PostAsJsonAsync(TripsEndpoint, request);

        // Assert
        response.EnsureSuccessStatusCode();

        await Retry.Handle<XunitException>(1000, 2)
            .ExecuteAsync(async () =>
            {
                var trip = await this.webApplicationFixture.Trips.GetTripByIdAsync(tripId);
                trip.Driver.Should().NotBeNull();
            });
    }
}

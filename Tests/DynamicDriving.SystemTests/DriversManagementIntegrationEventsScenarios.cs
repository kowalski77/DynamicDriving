using AutoFixture;
using DynamicDriving.Contracts.Drivers;
using DynamicDriving.SystemTests.Services;
using DynamicDriving.SystemTests.Support;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

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
        var client = this.webApplicationFixture.Drivers.TestServer.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(DriversEndpoint, request);

        // Assert
        response.EnsureSuccessStatusCode();

        var driverRepository = this.webApplicationFixture.Trips.TestServer.Services.GetRequiredService<IDriverRepository>();
        await Retry.Handle<Exception>(1000, 3)
            .ExecuteAsync(async () =>
            {
                var driver = (await driverRepository.GetAsync(driverId)).Value;
                driver.Name.Should().Be(request.Name);
            });
    }

    [Fact]
    public async Task Driver_is_assigned_in_trip_management_when_is_assigned_in_driver_management()
    {
        // Arrange
        var tripId = Guid.Parse(SystemTestsConstants.TripId);
        var request = this.fixture.Build<AssignDriverRequest>().With(x => x.TripId, tripId).Create();
        var client = this.webApplicationFixture.Drivers.TestServer.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(TripsEndpoint, request);

        // Assert
        response.EnsureSuccessStatusCode();

        var tripRepository = this.webApplicationFixture.Trips.TestServer.Services.GetRequiredService<ITripRepository>();
        await Retry.Handle<Exception>(1000, 3)
            .ExecuteAsync(async () =>
            {
                var trip = (await tripRepository.GetAsync(tripId)).Value;
                trip.Driver.Should().NotBeNull();
            });
    }
}

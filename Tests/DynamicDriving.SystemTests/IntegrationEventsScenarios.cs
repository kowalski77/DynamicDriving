using System.Net.Http.Json;
using AutoFixture;
using DynamicDriving.Models;
using DynamicDriving.SystemTests.Services;
using DynamicDriving.SystemTests.Support;
using FluentAssertions;

namespace DynamicDriving.SystemTests;

public class IntegrationEventsScenarios : IClassFixture<WebApplicationFixture>
{
    private const string DriversEndpoint = "/api/v1/Drivers";

    private readonly IFixture fixture;
    private readonly WebApplicationFixture webApplicationFixture;

    public IntegrationEventsScenarios(WebApplicationFixture webApplicationFixture)
    {
        this.fixture = new Fixture();
        this.webApplicationFixture = webApplicationFixture;
    }

    [Fact]
    public async Task Driver_is_created_in_trip_management_when_is_registered()
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
}

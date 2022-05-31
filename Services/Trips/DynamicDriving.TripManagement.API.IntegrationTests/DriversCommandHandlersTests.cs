using AutoFixture;
using DynamicDriving.Events;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class DriversCommandHandlersTests
{
    private readonly TestWebApplicationFactory factory;

    public DriversCommandHandlersTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_created_when_driver_created_event_is_received()
    {
        // Arrange
        _ = this.factory.CreateDefaultClient();
        var driverCreated = this.factory.Fixture.Create<DriverCreated>();
        var consumer = this.factory.GetConsumer<DriverCreated>();

        // Act
        await consumer.ExecuteAsync(driverCreated);

        // Assert
        var driver = await this.factory.GetDriverByIdAsync(driverCreated.DriverId);
        driver.Should().NotBeNull();
    }
}

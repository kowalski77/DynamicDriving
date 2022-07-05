using DynamicDriving.Events;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripsCommandHandlersTests
{
    private readonly TestWebApplicationFactory factory;

    public TripsCommandHandlersTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_assigned_to_trip_when_assigned_driver_event_is_received()
    {
        // Arrange
        //_ = this.factory.CreateDefaultClient();
        //var driverAssigned = new DriverAssigned(Guid.NewGuid(), Guid.Parse(IntegrationTestConstants.TripId), Guid.Parse(IntegrationTestConstants.DriverId));
        //var consumer = this.factory.GetConsumer<DriverAssigned>();

        //// Act
        //await consumer.ExecuteAsync(driverAssigned);

        //// Assert
        //var trip = await this.factory.GetTripByIdAsync(driverAssigned.TripId);
        //trip.Driver!.Id.Should().Be(driverAssigned.DriverId);

        Assert.True(false);
    }
}

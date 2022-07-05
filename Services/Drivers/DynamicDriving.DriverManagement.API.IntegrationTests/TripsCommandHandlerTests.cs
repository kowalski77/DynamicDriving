using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.Events;
using FluentAssertions;
using Xunit;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripsCommandHandlerTests
{
    private readonly TestWebApplicationFactory factory;

    public TripsCommandHandlerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Trip_is_stored_when_trip_confirmed_event_is_received()
    {
        // Arrange
        //_ = this.factory.Client;
        //var tripConfirmedEvent = this.factory.Fixture.Create<TripConfirmed>();
        //var consumer = this.factory.GetConsumer<TripConfirmed>();

        //// Act
        //await consumer.Consume(tripConfirmedEvent);

        //// Assert
        //var trip = await this.factory.GetTripByIdAsync(tripConfirmedEvent.Id);
        //trip.Should().NotBeNull();

        Assert.True(false);
    }
}

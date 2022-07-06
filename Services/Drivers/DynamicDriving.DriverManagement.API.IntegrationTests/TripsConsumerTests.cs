using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.DriverManagement.API.UseCases.Trips.Create;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.Events;
using FluentAssertions;
using MassTransit;
using Moq;
using Xunit;

namespace DynamicDriving.DriverManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripsConsumerTests
{
    private readonly TestWebApplicationFactory factory;

    public TripsConsumerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Trip_is_stored_when_trip_confirmed_event_is_received()
    {
        //Arrange
        var tripConfirmed = this.factory.Fixture.Create<TripCreated>();
        var consumeContextMock = new Mock<ConsumeContext<TripCreated>>();
        consumeContextMock.SetupGet(x => x.Message).Returns(tripConfirmed);

        _ = this.factory.Client;
        var consumer = this.factory.GetService<TripCreatedConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.GetService<ITripRepository>();
        var trip = await repository.GetAsync(tripConfirmed.TripId);
        trip.Should().NotBeNull();
    }
}

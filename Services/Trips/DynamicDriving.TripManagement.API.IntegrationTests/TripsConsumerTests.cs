using DynamicDriving.Events;
using DynamicDriving.TripManagement.API.UseCases.Trips.Assign;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using FluentAssertions;
using MassTransit;
using Moq;
using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class TripsConsumerTests
{
    private readonly TestWebApplicationFactory factory;

    public TripsConsumerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_assigned_to_trip_when_assigned_driver_event_is_received()
    {
        // Arrange
        var driverAssigned = new DriverAssigned(Guid.Parse(IntegrationTestConstants.TripId), Guid.Parse(IntegrationTestConstants.DriverId));
        var consumeContextMock = new Mock<ConsumeContext<DriverAssigned>>();
        consumeContextMock.SetupGet(x => x.Message).Returns(driverAssigned);

        _ = this.factory.Client;
        var consumer = this.factory.GetService<DriverAssignedConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.GetService<ITripRepository>();
        var trip = await repository.GetAsync(driverAssigned.TripId);
        trip.Value.Driver!.Id.Should().Be(driverAssigned.DriverId);
    }
}

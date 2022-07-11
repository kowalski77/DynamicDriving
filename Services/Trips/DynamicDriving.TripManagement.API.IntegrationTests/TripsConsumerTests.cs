using DynamicDriving.Contracts.Drivers;
using DynamicDriving.Contracts.Trips;
using DynamicDriving.TripManagement.API.UseCases.Trips.Assign;
using DynamicDriving.TripManagement.API.UseCases.Trips.Confirm;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
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
        var dbContext = this.factory.TestServer.Services.GetRequiredService<TripManagementContext>();
        var trip = dbContext.Trips.First();

        var driverAssigned = new DriverAssigned(trip.Id, Guid.Parse(IntegrationTestConstants.DriverId));
        var consumeContextMock = new Mock<ConsumeContext<DriverAssigned>>();
        consumeContextMock.SetupGet(x => x.Message).Returns(driverAssigned);
        
        var consumer = this.factory.TestServer.Services.GetRequiredService<DriverAssignedConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.TestServer.Services.GetRequiredService<ITripRepository>();
        var maybeTrip = await repository.GetAsync(driverAssigned.TripId);
        maybeTrip.Value.Driver!.Id.Should().Be(driverAssigned.DriverId);
    }

    [Fact]
    public async Task Trip_is_confirmed_when_confirm_trip_event_is_received()
    {
        // Arrange
        var dbContext = this.factory.TestServer.Services.GetRequiredService<TripManagementContext>();
        var trip = dbContext.Trips.First();
        var confirmTrip = new ConfirmTrip(trip.Id, Guid.NewGuid());
        var consumeContextMock = new Mock<ConsumeContext<ConfirmTrip>>();
        consumeContextMock.SetupGet(x => x.Message).Returns(confirmTrip);

        var consumer = this.factory.TestServer.Services.GetRequiredService<ConfirmTripConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.TestServer.Services.GetRequiredService<ITripRepository>();
        var maybeTrip = await repository.GetAsync(confirmTrip.TripId);
        maybeTrip.Value.TripStatus.Should().Be(TripStatus.Confirmed);
    }
}

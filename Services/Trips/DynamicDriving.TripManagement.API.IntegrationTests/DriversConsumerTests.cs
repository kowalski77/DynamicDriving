using AutoFixture;
using DynamicDriving.Contracts.Drivers;
using DynamicDriving.TripManagement.API.UseCases.Drivers.Create;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DynamicDriving.TripManagement.API.IntegrationTests;

[Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
public class DriversConsumerTests
{
    private readonly TestWebApplicationFactory factory;

    public DriversConsumerTests(TestWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task Driver_is_created_when_driver_created_event_is_received()
    {
        // Arrange
        var driverCreated = this.factory.Fixture.Create<DriverCreated>();
        var consumeContextMock = new Mock<ConsumeContext<DriverCreated>>();
        consumeContextMock.SetupGet(x => x.Message).Returns(driverCreated);

        var consumer = this.factory.TestServer.Services.GetRequiredService<DriverCreatedConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.Services.GetRequiredService<IDriverRepository>();

        var driver = await repository.GetAsync(driverCreated.DriverId);
        driver.Should().NotBeNull();
    }
}

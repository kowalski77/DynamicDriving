﻿using System.Threading.Tasks;
using AutoFixture;
using DynamicDriving.Contracts.Trips;
using DynamicDriving.DriverManagement.API.UseCases.Trips.Create;
using DynamicDriving.DriverManagement.Core.Trips;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
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

        var consumer = this.factory.TestServer.Services.GetRequiredService<TripCreatedConsumer>();

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        var repository = this.factory.TestServer.Services.GetRequiredService<ITripRepository>();
        var trip = await repository.GetAsync(tripConfirmed.TripId);
        trip.Should().NotBeNull();
    }
}

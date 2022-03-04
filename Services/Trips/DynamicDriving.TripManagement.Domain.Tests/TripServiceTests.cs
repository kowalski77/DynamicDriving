using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using DynamicDriving.TripManagement.Domain.CarsAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace DynamicDriving.TripManagement.Domain.Tests;

public class TripServiceTests
{
    [Theory, TestServiceDataSource]
    public async Task Draft_trip_is_created_when_valid_coordinates(
        [Frozen] Mock<ILocationProvider> locationProviderMock,
        [Frozen] Mock<ILocationRepository> locationRepositoryMock,
        Location location,
        User user, Car car, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync(location);
        locationRepositoryMock.Setup(x => x.GetLocations())
            .Returns(new List<Location> { location });

        // Act
        var result = await sut.CreateDraftTripAsync(user, car, pickUp, origin, destination);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.TripStatus.Should().Be(TripStatus.Draft);
    }
}

public class TestServiceDataSourceAttribute : CustomDataSourceAttribute
{
    public TestServiceDataSourceAttribute() : base(new ValidatorCustomization())
    {
    }

    private class ValidatorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<ICoordinatesValidator>(fixture.Create<CoordinatesValidator>);
        }
    }
}

﻿using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Domain.Tests.Trips;

public class TripServiceTests
{
    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_created_when_valid_coordinates(
        [Frozen] Mock<ICityProvider> cityProviderMock,
        [Frozen] Mock<ILocationRepository> locationRepositoryMock,
        City city,
        Location location,
        Guid tripId,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        cityProviderMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync(city);
        locationRepositoryMock.Setup(x => x.GetLocationByCityNameAsync(city.Name, CancellationToken.None))
            .ReturnsAsync(location);

        // Act
        var result = await sut.CreateDraftTripAsync(tripId, user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.TripStatus.Should().Be(TripStatus.Draft);
    }

    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_not_created_when_invalid_location_coordinates(
        [Frozen] Mock<ICityProvider> cityProviderMock,
        Guid tripId,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        cityProviderMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync((Maybe<City>)null!);

        // Act
        var result = await sut.CreateDraftTripAsync(tripId, user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error!.Code.Should().Be(DomainErrorConstants.InvalidCityCode);
        result.Error!.Message.Should().Be(string.Format(DomainErrorConstants.InvalidCoordinatesMessage, origin.Latitude, origin.Longitude));
    }

    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_not_created_when_location_coordinates_does_not_belong_to_a_valid_city(
        [Frozen] Mock<ICityProvider> cityProviderMock,
        [Frozen] Mock<ILocationRepository> locationRepositoryMock,
        City city,
        Guid tripId,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        cityProviderMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync(city);
        locationRepositoryMock.Setup(x => x.GetLocationByCityNameAsync(city.Name, CancellationToken.None))
            .ReturnsAsync(new Maybe<Location>());

        // Act
        var result = await sut.CreateDraftTripAsync(tripId, user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error!.Code.Should().Be(DomainErrorConstants.InvalidCityCode);
        result.Error!.Message.Should().Be(string.Format(DomainErrorConstants.InvalidCityMessage, city.Name));
    }

    private class TripServiceDataSourceAttribute : CustomDataSourceAttribute
    {
        public TripServiceDataSourceAttribute() : base(new ValidatorCustomization())
        {
        }

        private class ValidatorCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Register<ILocationService>(fixture.Create<LocationService>);
                var coordinates = Coordinates.CreateInstance(10, 10);
                fixture.Inject(coordinates.Value);
            }
        }
    }
}

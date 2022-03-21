using DynamicDriving.SharedKernel;
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
        [Frozen] Mock<ILocationProvider> locationProviderMock,
        [Frozen] Mock<ILocationRepository> locationRepositoryMock,
        Location location,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync(location);
        locationRepositoryMock.Setup(x => x.GetLocationsAsync(CancellationToken.None))
            .ReturnsAsync(new[] { location });

        // Act
        var result = await sut.CreateDraftTripAsync(user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.TripStatus.Should().Be(TripStatus.Draft);
    }

    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_not_created_when_invalid_location_coordinates(
        [Frozen] Mock<ILocationProvider> locationProviderMock,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync((Maybe<Location>)null!);

        // Act
        var result = await sut.CreateDraftTripAsync(user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error!.Code.Should().Be(DomainErrorConstants.InvalidCoordinatesCode);
        result.Error!.Message.Should().Be(string.Format(DomainErrorConstants.InvalidCoordinatesMessage, origin.Latitude, origin.Longitude));
    }

    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_not_created_when_location_coordinates_does_not_belong_to_a_valid_city(
        [Frozen] Mock<ILocationProvider> locationProviderMock,
        [Frozen] Mock<ILocationRepository> locationRepositoryMock,
        Location location,
        Location otherLocation,
        User user, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        locationProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<Coordinates>()))
            .ReturnsAsync(location);
        locationRepositoryMock.Setup(x => x.GetLocationsAsync(CancellationToken.None))
            .ReturnsAsync(new[] { otherLocation });

        // Act
        var result = await sut.CreateDraftTripAsync(user, pickUp, origin, destination, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error!.Code.Should().Be(DomainErrorConstants.InvalidCoordinatesCode);
        result.Error!.Message.Should().Be(string.Format(DomainErrorConstants.InvalidCityCoordinatesMessage, origin.Latitude, origin.Longitude));
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
                fixture.Register<ICoordinatesValidator>(fixture.Create<CoordinatesValidator>);
                var coordinates = Coordinates.CreateInstance(10, 10);
                fixture.Inject(coordinates.Value);
            }
        }
    }
}

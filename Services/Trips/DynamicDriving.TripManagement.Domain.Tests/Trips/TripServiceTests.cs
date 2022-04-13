using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Domain.Tests.Trips;

public class TripServiceTests
{
    [Theory, TripServiceDataSource]
    public async Task Draft_trip_is_created_when_valid_coordinates(
        [Frozen] Mock<ICoordinatesAgent> coordinatesAgentMock,
        [Frozen] Mock<ICityRepository> cityRepositoryMock,
        string cityName,
        string locationName,
        City city,
        UserId userId, DateTime pickUp, Coordinates origin, Coordinates destination,
        TripService sut)
    {
        // Arrange
        coordinatesAgentMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync(cityName);
        coordinatesAgentMock.Setup(x => x.GetLocationByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync(locationName);
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(cityName, CancellationToken.None))
            .ReturnsAsync(city);

        // Act
        var result = await sut.CreateDraftTripAsync(Guid.NewGuid(), userId, pickUp, origin, destination);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Origin.Name.Should().Be(locationName);
        result.Value.Destination.Name.Should().Be(locationName);
        result.Value.PickUp.Should().Be(pickUp);
        result.Value.UserId.Value.Should().Be(userId.Value);
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
                fixture.Register<ILocationFactory>(fixture.Create<LocationFactory>);
                var coordinates = Coordinates.CreateInstance(10, 10);
                fixture.Inject(coordinates.Value);
                var userId = UserId.CreateInstance(Guid.NewGuid());
                fixture.Inject(userId.Value);
            }
        }
    }
}

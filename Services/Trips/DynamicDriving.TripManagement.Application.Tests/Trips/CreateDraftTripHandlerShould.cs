using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Application.Tests.Trips;

public class CreateDraftTripHandlerShould
{
    [Theory, HandlerDataSource]
    public async Task Return_a_draft_trip_identifier_when_command_is_valid(
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        [Frozen] Mock<ICityRepository> cityRepositoryMock,
        [Frozen] Mock<ICoordinatesAgent> coordinatesAgentMock,
        CreateDraftTrip command,
        string cityName, string locationName,
        City city,
        CreateDraftTripHandler sut)
    {
        // Arrange
        tripRepositoryMock.Setup(x => x.Add(It.IsAny<Trip>())).Returns<Trip>(x=>x);
        coordinatesAgentMock.Setup(x => x.GetDistanceInKmBetweenCoordinates(It.IsAny<Coordinates>(), It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync(5);
        coordinatesAgentMock.Setup(x => x.GetCityByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync((Maybe<string>)cityName);
        coordinatesAgentMock.Setup(x => x.GetLocationByCoordinatesAsync(It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync((Maybe<string>)locationName);
        cityRepositoryMock.Setup(x => x.GetCityByNameAsync(cityName, CancellationToken.None))
            .ReturnsAsync(city);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();
        result.Value.Id.Should().NotBeEmpty();
        tripRepositoryMock.Verify(x => x.Add(It.IsAny<Trip>()), Times.Once);
        tripRepositoryMock.Verify(x => x.UnitOfWork.SaveEntitiesAsync(CancellationToken.None), Times.Once);
    }

    private class HandlerDataSourceAttribute : CustomDataSourceAttribute
    {
        public HandlerDataSourceAttribute() : base(new ValidatorCustomization())
        {
        }

        private class ValidatorCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<CreateDraftTrip>(x => x
                    .With(y => y.OriginLatitude, 10)
                    .With(y => y.OriginLongitude, 10)
                    .With(y => y.DestinationLatitude, 10)
                    .With(y => y.DestinationLongitude, 10));

                fixture.Register<ITripService>(fixture.Create<TripService>);
                fixture.Register<ILocationFactory>(fixture.Create<LocationFactory>);
                fixture.Register<ITripValidator>(fixture.Create<TripValidator>);
            }
        }
    }
}

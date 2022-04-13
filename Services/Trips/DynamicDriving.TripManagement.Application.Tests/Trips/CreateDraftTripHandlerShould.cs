using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Application.Tests.Trips;

public class CreateDraftTripHandlerShould
{
    [Theory, HandlerDataSource]
    public async Task Return_a_draft_trip_identifier_when_command_is_valid(
        [Frozen] Mock<ITripService> tripServiceMock,
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        CreateDraftTrip command,
        Trip trip,
        CreateDraftTripHandler sut)
    {
        // Arrange
        tripServiceMock.Setup(x => x
            .CreateDraftTripAsync(command.TripId, It.IsAny<UserId>(), command.PickUp, It.IsAny<Coordinates>(), It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(trip));
        tripRepositoryMock.Setup(x => x.Add(trip)).Returns(trip);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.ErrorResult.Should().BeNull();
        result.Value.Id.Should().Be(trip.Id);
        tripRepositoryMock.Verify(x => x.Add(trip), Times.Once);
        tripRepositoryMock.Verify(x => x.UnitOfWork.SaveEntitiesAsync(CancellationToken.None), Times.Once);
    }

    [Theory, HandlerDataSource]
    public async Task Return_error_when_coordinates_are_not_valid(
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        CreateDraftTrip command,
        CreateDraftTripHandler sut)
    {
        // Arrange
        var commandWithOutOfRangeLatitude = command with { OriginLatitude = -150 };

        // Act
        var result = await sut.Handle(commandWithOutOfRangeLatitude, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorResult!.Code.Should().Be(CoordinatesErrorConstants.OutOfRangeCoordinatesCode);
        tripRepositoryMock.Verify(x => x.Add(It.IsAny<Trip>()), Times.Never);
        tripRepositoryMock.Verify(x => x.UnitOfWork.SaveEntitiesAsync(CancellationToken.None), Times.Never);
    }

    [Theory, HandlerDataSource]
    public async Task Return_error_when_cannot_create_a_draft_trip(
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        [Frozen] Mock<ITripService> tripServiceMock,
        string errorCode, string errorMessage,
        CreateDraftTrip command,
        CreateDraftTripHandler sut)
    {
        // Arrange
        tripServiceMock.Setup(x => x.CreateDraftTripAsync(command.TripId, It.IsAny<UserId>(), command.PickUp, It.IsAny<Coordinates>(), It.IsAny<Coordinates>(), CancellationToken.None))
            .ReturnsAsync(Result.Fail<Trip>(new ErrorResult(errorCode, errorMessage)));

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorResult!.Code.Should().Be(errorCode);
        result.ErrorResult!.Message.Should().Be(errorMessage);
        tripRepositoryMock.Verify(x => x.Add(It.IsAny<Trip>()), Times.Never);
        tripRepositoryMock.Verify(x => x.UnitOfWork.SaveEntitiesAsync(CancellationToken.None), Times.Never);
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

                var userId = UserId.CreateInstance(Guid.NewGuid()).Value;
                var coordinates = Coordinates.CreateInstance(10, 10);
                var originLocation = new Location(Guid.NewGuid(), fixture.Create<string>(), fixture.Create<City>(), coordinates.Value);
                var destinationLocation = new Location(Guid.NewGuid(), fixture.Create<string>(), fixture.Create<City>(), coordinates.Value);

                var trip = new Trip(Guid.NewGuid(), userId, fixture.Create<DateTime>(), originLocation, destinationLocation);
                fixture.Inject(trip);
            }
        }
    }
}

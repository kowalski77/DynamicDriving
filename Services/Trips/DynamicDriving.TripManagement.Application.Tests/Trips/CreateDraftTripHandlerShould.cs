using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Application.Tests.Trips;

public class CreateDraftTripHandlerShould
{
    [Theory, HandlerDataSource]
    public async Task Create_a_draft_trip_when_coordinates_are_valid(
        [Frozen] Mock<ITripService> tripServiceMock,
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        CreateDraftTrip command,
        User user,
        Trip trip,
        CreateDraftTripHandler sut)
    {
        // Arrange
        userRepositoryMock.Setup(x => x.GetAsync(command.UserId, CancellationToken.None))
            .ReturnsAsync(user);
        tripServiceMock.Setup(x => x
            .CreateDraftTripAsync(user, command.PickUp, It.IsAny<Coordinates>(), It.IsAny<Coordinates>(), CancellationToken.None))
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

                var coordinates = Coordinates.CreateInstance(10, 10);
                var user = new User();
                fixture.Inject(user);
                var trip = new Trip(user, fixture.Create<DateTime>(), coordinates.Value, coordinates.Value);
                fixture.Inject(trip);
            }
        }
    }
}

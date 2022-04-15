using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.Application.Tests.Trips;

public class ConfirmTripHandlerShould
{
    [Theory, HandlerDataSource]
    public async Task Confirm_trip_when_status_is_draft(
        [Frozen] Mock<ITripRepository> tripRepositoryMock,
        ConfirmTrip command,
        Trip trip,
        ConfirmTripHandler sut)
    {
        // Arrange
        tripRepositoryMock.Setup(x => x.GetAsync(command.TripId, CancellationToken.None))
            .ReturnsAsync(trip);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        tripRepositoryMock.Verify(x => x.GetAsync(command.TripId, CancellationToken.None), Times.Once);
        tripRepositoryMock.Verify(x => x.UnitOfWork.SaveEntitiesAsync(CancellationToken.None), Times.Once);
        result.Error.Should().BeNull();
        result.Success.Should().BeTrue();
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
                var coordinates = Coordinates.CreateInstance(10, 10).Value;
                fixture.Inject(coordinates);
                var userId = UserId.CreateInstance(Guid.NewGuid()).Value;
                fixture.Inject(userId);
            }
        }
    }
}

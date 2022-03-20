using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Application.Users;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Application.IntegrationTests.Trips.Commands;

public class CreateDraftTripHandlerShould
{
    [Theory, HandlerDataSource]
    public async Task CreateDraftTrip_WhenValidCoordinates(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        CreateDraftTrip command,
        User user,
        CreateDraftTripHandler sut)
    {
        // Arrange
        userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(user);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert

    }

    private class HandlerDataSourceAttribute : CustomDataSourceAttribute
    {
        public HandlerDataSourceAttribute() : base(new TripServiceCustomization())
        {
        }

        private class TripServiceCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Register<ITripService>(fixture.Create<TripService>);
                fixture.Register<ICoordinatesValidator>(fixture.Create<CoordinatesValidator>);
                var command = fixture.Build<CreateDraftTrip>()
                    .With(x => x.OriginLatitude, 20)
                    .With(x => x.OriginLongitude, 20)
                    .With(x => x.DestinationLatitude, 10)
                    .With(x => x.DestinationLongitude, 10)
                    .Create();
                fixture.Inject(command);
            }
        }
    }
}

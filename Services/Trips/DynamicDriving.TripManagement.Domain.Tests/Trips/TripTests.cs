using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Domain.Tests.Trips;

public class TripTests
{
    private readonly IFixture fixture;

    public TripTests()
    {
        this.fixture = new Fixture();
        var userId = UserId.CreateInstance(Guid.NewGuid()).Value;
        this.fixture.Inject(userId);
        var coordinates = Coordinates.CreateInstance(10, 10).Value;
        this.fixture.Inject(coordinates);
    }

    [Fact]
    public void Driver_is_assigned_to_a_trip()
    {
        // Arrange
        var sut = this.fixture.Create<Trip>();
        var driver = this.fixture.Create<Driver>();

        // Act
        sut.Assign(driver);

        // Assert
        sut.Driver.Should().Be(driver);
    }
}

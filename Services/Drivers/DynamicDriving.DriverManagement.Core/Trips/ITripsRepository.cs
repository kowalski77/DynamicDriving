namespace DynamicDriving.DriverManagement.Core.Trips;

public interface ITripsRepository
{
    Task<Trip> AddAsync(Trip trip, CancellationToken cancellationToken = default);
}

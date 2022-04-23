namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public interface ITripsRepository
{
    Task<Trip> AddAsync(Trip trip, CancellationToken cancellationToken = default);
}

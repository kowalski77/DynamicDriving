using DynamicDriving.DriverManagement.Core.Trips;
using Microsoft.Extensions.Logging;

namespace DynamicDriving.DriverManagement.Infrastructure;

public class TripsRepository : ITripsRepository
{
    private readonly ILogger<TripsRepository> logger;

    public TripsRepository(ILogger<TripsRepository> logger)
    {
        this.logger = logger;
    }

    public Task<Trip> AddAsync(Trip trip, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation($"Trip repository method called with: {trip}");

        return Task.FromResult(trip);
    }
}

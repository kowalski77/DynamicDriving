using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class TripReadRepository : ITripReadRepository
{
    private readonly TripManagementContext context;

    public TripReadRepository(TripManagementContext context)
    {
        this.context = Guards.ThrowIfNull(context);
    }

    public async Task<Maybe<Trip>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trip = await this.context.Trips
            .AsNoTracking()
            .Include(x => x.Origin).ThenInclude(x => x.City)
            .Include(x => x.Destination).ThenInclude(x => x.City)
            .Include(x => x.Driver)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return trip;
    }

    public async Task<IReadOnlyList<TripDto>> GetTripsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var trips = await this.context.Trips
            .AsNoTracking()
            .Select(x => new TripDto(userId, x.Driver.Name, x.PickUp, // TODO: x.Driver?.name --> error??? WTF!!!
            new TripByIdLocationDto(x.Origin.Name, x.Origin.City.Name, x.Origin.Coordinates.Latitude, x.Origin.Coordinates.Longitude),
            new TripByIdLocationDto(x.Destination.Name, x.Destination.City.Name, x.Destination.Coordinates.Latitude, x.Destination.Coordinates.Longitude)))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return trips;
    }
}

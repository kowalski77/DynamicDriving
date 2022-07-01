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

    public async Task<IReadOnlyList<TripSummaryDto>> GetTripsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var trips = await this.context.Trips
            .AsNoTracking()
            .Select(x => new TripSummaryDto(
                x.UserId.Value, 
                x.Driver.Name, // TODO: WTF?
                x.TripStatus.ToString(), 
                x.Origin.Name, 
                x.Destination.Name))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return trips;
    }
}

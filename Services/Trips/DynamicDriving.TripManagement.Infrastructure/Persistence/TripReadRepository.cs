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

    public async Task<Maybe<Trip>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var trip = await this.context.Trips
            .AsNoTracking()
            .Include(x => x.Origin).ThenInclude(x => x.City)
            .Include(x => x.Destination).ThenInclude(x => x.City)
            .Include(x => x.Driver)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return trip;
    }
}

using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class TripRepository : BaseRepository<Trip>, ITripRepository
{
    private readonly TripManagementContext context;

    public TripRepository(TripManagementContext context) : base(context)
    {
        this.context = context;
    }

    public override async Task<Maybe<Trip>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var trip = await this.context.Trips
            .Include(x => x.Origin).ThenInclude(x => x.City)
            .Include(x => x.Destination).ThenInclude(x => x.City)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return trip;
    }
}

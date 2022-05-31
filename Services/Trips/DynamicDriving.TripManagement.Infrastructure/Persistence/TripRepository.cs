using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

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
        var trip = await this.context.Trips.FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false);
        if (trip is null)
        {
            return trip;
        }

        await this.context.Entry(trip).Reference(x => x.Origin).LoadAsync(cancellationToken).ConfigureAwait(false);
        await this.context.Entry(trip).Reference(x => x.Destination).LoadAsync(cancellationToken).ConfigureAwait(false);
        await this.context.Entry(trip).Reference(x => x.Driver).LoadAsync(cancellationToken).ConfigureAwait(false);

        return trip;
    }
}

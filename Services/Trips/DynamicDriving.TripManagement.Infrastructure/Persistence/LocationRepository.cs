using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    private readonly TripManagementContext context;

    public LocationRepository(TripManagementContext context) : base(context)
    {
        this.context = Guards.ThrowIfNull(context);
    }

    public async Task<Maybe<Location>> GetLocationByCityNameAsync(string city, CancellationToken cancellationToken = default)
    {
        return await this.context.Locations
            .FirstOrDefaultAsync(x => x.City.Name == city, cancellationToken)
            .ConfigureAwait(false);
    }
}

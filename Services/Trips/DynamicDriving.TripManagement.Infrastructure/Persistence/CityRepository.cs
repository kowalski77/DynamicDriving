using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class CityRepository : ICityRepository
{
    private readonly TripManagementContext context;

    public CityRepository(TripManagementContext context)
    {
        this.context = Guards.ThrowIfNull(context);
    }

    public async Task<Maybe<City>> GetCityByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.context.Cities
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken)
            .ConfigureAwait(false);
    }
}

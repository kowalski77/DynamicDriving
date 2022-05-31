using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public class DriverRepository : BaseRepository<Driver>, IDriverRepository
{
    private readonly TripManagementContext context;

    public DriverRepository(TripManagementContext context) : base(context)
    {
        this.context = context;
    }

    public override async Task<Maybe<Driver>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driver = await this.context.Drivers.FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false);
        if (driver is null)
        {
            return driver;
        }

        await this.context.Entry(driver).Reference(x => x.Car).LoadAsync(cancellationToken).ConfigureAwait(false);

        return driver;
    }
}

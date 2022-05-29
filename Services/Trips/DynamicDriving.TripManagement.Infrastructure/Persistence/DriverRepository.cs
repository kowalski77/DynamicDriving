using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public class DriverRepository : BaseRepository<Driver>, IDriverRepository
{
    public DriverRepository(TransactionContext context) : base(context)
    {
    }
}

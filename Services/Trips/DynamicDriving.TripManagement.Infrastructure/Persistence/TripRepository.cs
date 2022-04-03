using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class TripRepository : BaseRepository<Trip>, ITripRepository
{
    public TripRepository(TripManagementContext context) : base(context)
    {
    }
}

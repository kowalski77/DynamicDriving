using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.TripsAggregate;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class TripRepository : ITripRepository
{
    public IUnitOfWork UnitOfWork { get; }

    public Trip Add(Trip item)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<Trip>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

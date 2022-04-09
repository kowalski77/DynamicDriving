using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public interface ITripReadRepository
{
    Task<Maybe<Trip>> GetById(Guid id, CancellationToken cancellationToken = default);
}

using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public interface ITripReadRepository
{
    Task<Maybe<Trip>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<TripDto>> GetTripsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

using DynamicDriving.SharedKernel;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public interface ITripReadRepository
{
    Task<Maybe<Trip>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<TripSummaryDto>> GetTripsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

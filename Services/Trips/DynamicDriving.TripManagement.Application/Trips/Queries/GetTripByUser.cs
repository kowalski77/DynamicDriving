using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public record GetTripByUser(Guid UserId) : IRequest<IReadOnlyList<TripSummaryDto>>;

public sealed class GetTripByUserHandler : IRequestHandler<GetTripByUser, IReadOnlyList<TripSummaryDto>>
{
    private readonly ITripReadRepository repository;

    public GetTripByUserHandler(ITripReadRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IReadOnlyList<TripSummaryDto>> Handle(GetTripByUser request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        return await this.repository.GetTripsByUserIdAsync(request.UserId, cancellationToken);
    }
}

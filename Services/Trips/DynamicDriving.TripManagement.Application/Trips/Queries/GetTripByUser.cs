using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public record GetTripByUser(Guid UserId) : IRequest<IReadOnlyList<TripDto>>;

public sealed class GetTripByUserHandler : IRequestHandler<GetTripByUser, IReadOnlyList<TripDto>>
{
    private readonly ITripReadRepository repository;

    public GetTripByUserHandler(ITripReadRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IReadOnlyList<TripDto>> Handle(GetTripByUser request, CancellationToken cancellationToken)
    {
        _ = Guards.ThrowIfNull(request);

        return await this.repository.GetTripsByUserIdAsync(request.UserId, cancellationToken);
    }
}

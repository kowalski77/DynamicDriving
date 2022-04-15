using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public sealed class GetTripByIdHandler : IRequestHandler<GetTripById, Result<TripByIdDto>>
{
    private readonly ITripReadRepository repository;

    public GetTripByIdHandler(ITripReadRepository repository)
    {
        this.repository = Guards.ThrowIfNull(repository);
    }

    public async Task<Result<TripByIdDto>> Handle(GetTripById request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await Result.Init
            .OnSuccess(async () => await this.GetTripById(request.Id, cancellationToken))
            .OnSuccess(trip => trip.AsDto());

        return resultModel;
    }

    private async Task<Result<Trip>> GetTripById(Guid id, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.repository.GetById(id, cancellationToken);

        return maybeTrip.TryGetValue(out var trip) ? 
            trip : 
            GeneralErrors.NotFound(id, nameof(Trip.Id));
    }
}

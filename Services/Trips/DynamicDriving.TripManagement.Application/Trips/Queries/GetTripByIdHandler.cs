using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public sealed class GetTripByIdHandler : IRequestHandler<GetTripById, IResultModel<TripByIdDto>>
{
    private readonly ITripReadRepository repository;

    public GetTripByIdHandler(ITripReadRepository repository)
    {
        this.repository = Guards.ThrowIfNull(repository);
    }

    public async Task<IResultModel<TripByIdDto>> Handle(GetTripById request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await ResultModel.Init
            .OnSuccess(async () => await this.GetTripById(request.Id, cancellationToken))
            .OnSuccess(trip => trip.AsDto());

        return resultModel;
    }

    private async Task<IResultModel<Trip>> GetTripById(Guid id, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.repository.GetById(id, cancellationToken);

        return maybeTrip.TryGetValue(out var trip) ? 
            ResultModel.Ok(trip) : 
            ResultModel.Fail<Trip>(GeneralErrors.NotFound(id, nameof(Trip.Id)));
    }
}

using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public class ConfirmTripHandler : ICommandHandler<ConfirmTrip, IResultModel>
{
    private readonly ITripRepository tripRepository;

    public ConfirmTripHandler(ITripRepository tripRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<IResultModel> Handle(ConfirmTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await ResultModel.Init
            .OnSuccess(async () => await this.GetTripByIdAsync(request.TripId, cancellationToken))
            .OnSuccess(async trip => await this.ConfirmTripAsync(trip, cancellationToken));

        return resultModel;
    }

    private async Task<IResultModel<Trip>> GetTripByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.tripRepository.GetAsync(id, cancellationToken);

        return !maybeTrip.TryGetValue(out var trip) ? 
            ResultModel.Fail<Trip>(GeneralErrors.NotFound(id, nameof(Trip.Id))) : 
            ResultModel.Ok(trip);
    }

    private async Task<IResultModel> ConfirmTripAsync(Trip trip, CancellationToken cancellationToken)
    {
        var result = trip.CanConfirm();
        if (!result.Success)
        {
            return ResultModel.Fail(result.Error);
        }

        trip.Confirm();
        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultModel.Ok();
    }
}

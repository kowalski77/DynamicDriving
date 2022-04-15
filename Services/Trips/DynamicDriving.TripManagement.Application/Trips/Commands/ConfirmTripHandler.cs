using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public class ConfirmTripHandler : ICommandHandler<ConfirmTrip, Result>
{
    private readonly ITripRepository tripRepository;

    public ConfirmTripHandler(ITripRepository tripRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<Result> Handle(ConfirmTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await Result.Init
            .OnSuccess(async () => await this.GetTripByIdAsync(request.TripId, cancellationToken))
            .OnSuccess(async trip => await this.ConfirmTripAsync(trip, cancellationToken));

        return resultModel;
    }

    private async Task<Result<Trip>> GetTripByIdAsync(Guid tripId, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.tripRepository.GetAsync(tripId, cancellationToken);

        return !maybeTrip.TryGetValue(out var trip) ? 
            GeneralErrors.NotFound(tripId, nameof(tripId)) :
            trip;
    }

    private async Task<Result> ConfirmTripAsync(Trip trip, CancellationToken cancellationToken)
    {
        var result = trip.CanConfirm();
        if (result.Failure)
        {
            return result.Error!;
        }

        trip.Confirm();
        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Ok();
    }
}

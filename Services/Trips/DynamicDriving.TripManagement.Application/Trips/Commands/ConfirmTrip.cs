using DynamicDriving.Contracts.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed record ConfirmTrip(Guid TripId, Guid CorrelationId) : ICommand<Unit>;

public sealed class ConfirmTripHandler : ICommandHandler<ConfirmTrip, Unit>
{
    private readonly ITripRepository tripRepository;
    private readonly IOutboxService outboxService;

    public ConfirmTripHandler(ITripRepository tripRepository, IOutboxService outboxService)
    {
        this.tripRepository = tripRepository;
        this.outboxService = outboxService;
    }

    public async Task<Unit> Handle(ConfirmTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await Result.Init
            .Do(async () => await this.GetTripByIdAsync(request.TripId, cancellationToken))
            .OnSuccess(async trip => await this.ConfirmTripAsync(trip, request.CorrelationId, cancellationToken));

        return Unit.Value;
    }

    private async Task<Result<Trip>> GetTripByIdAsync(Guid tripId, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.tripRepository.GetAsync(tripId, cancellationToken);

        return maybeTrip.HasValue ?
            maybeTrip.Value :
            GeneralErrors.NotFound(tripId, nameof(tripId));
    }

    private async Task<Result> ConfirmTripAsync(Trip trip, Guid correlationId, CancellationToken cancellationToken)
    {
        trip.Confirm();

        await this.outboxService.AddIntegrationEventAsync(GetTripCreatedEvent(trip), cancellationToken);
        await this.outboxService.AddIntegrationEventAsync(new TripConfirmed(correlationId), cancellationToken);

        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Result.Ok();
    }

    private static TripCreated GetTripCreatedEvent(Trip trip)
    {
        return new TripCreated(
            trip.Id,
            trip.PickUp,
            trip.Origin.Coordinates.Latitude,
            trip.Origin.Coordinates.Longitude,
            trip.Destination.Coordinates.Latitude,
            trip.Destination.Coordinates.Longitude);
    }
}

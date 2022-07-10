using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Application.Trips.Exceptions;
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

        var trip = await this.tripRepository.GetAsync(request.TripId, cancellationToken);
        if (trip.HasNoValue)
        {
            throw new TripNotFoundException(request.TripId);
        }

        trip.Value.Confirm();

        await this.outboxService.AddIntegrationEventAsync(GetTripCreatedEvent(trip.Value), cancellationToken);
        await this.outboxService.AddIntegrationEventAsync(new TripConfirmed(request.CorrelationId), cancellationToken);

        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
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

using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Application.Trips.Exceptions;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public record InvalidateTrip(Guid TripId, Guid CorrelationId) : ICommand<Unit>;

public sealed class InvalidateTripHandler : ICommandHandler<InvalidateTrip, Unit>
{
    private readonly ITripRepository tripRepository;
    private readonly IOutboxService outboxService;

    public InvalidateTripHandler(ITripRepository tripRepository, IOutboxService outboxService)
    {
        this.tripRepository = tripRepository;
        this.outboxService = outboxService;
    }

    public async Task<Unit> Handle(InvalidateTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var trip = await this.tripRepository.GetAsync(request.TripId, cancellationToken);
        if (trip.HasNoValue)
        {
            throw new TripNotFoundException(request.TripId);
        }

        trip.Value.Invalidate();

        await this.outboxService.AddIntegrationEventAsync(new TripInvalidated(trip.Value.Id, request.CorrelationId), cancellationToken);

        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}

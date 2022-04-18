using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Events;

namespace DynamicDriving.TripManagement.Application.Trips.Events;

public sealed class TripConfirmedHandler : IDomainNotificationHandler<TripConfirmed>
{
    private readonly IOutboxService outboxService;

    public TripConfirmedHandler(IOutboxService outboxService)
    {
        this.outboxService = Guards.ThrowIfNull(outboxService);
    }

    public async Task Handle(TripConfirmed notification, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(notification);

        await this.outboxService.AddIntegrationEventAsync(notification.AsIntegrationEvent(), cancellationToken).ConfigureAwait(false);
    }
}

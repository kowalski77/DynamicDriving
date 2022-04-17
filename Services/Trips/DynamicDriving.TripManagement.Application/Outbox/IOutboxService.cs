using DynamicDriving.Events;

namespace DynamicDriving.TripManagement.Application.Outbox;

public interface IOutboxService
{
    Task AddIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class, IIntegrationEvent;

    Task PublishIntegrationEventsAsync(Guid transactionId, CancellationToken cancellationToken = default);
}

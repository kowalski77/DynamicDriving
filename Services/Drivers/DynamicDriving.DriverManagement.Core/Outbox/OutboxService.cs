using DynamicDriving.EventBus;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;

namespace DynamicDriving.DriverManagement.Core.Outbox;

public class OutboxService : IOutboxService
{
    private readonly IEventBusMessagePublisher eventBusMessagePublisher;
    private readonly IOutboxRepository outboxRepository;

    public OutboxService(IEventBusMessagePublisher eventBusMessagePublisher, IOutboxRepository outboxRepository)
    {
        this.eventBusMessagePublisher = Guards.ThrowIfNull(eventBusMessagePublisher);
        this.outboxRepository = Guards.ThrowIfNull(outboxRepository);
    }

    public Task PublishIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class, IIntegrationEvent
    {
        Guards.ThrowIfNull(integrationEvent);

        return this.TryPublishIntegrationEventAsync(integrationEvent, cancellationToken);
    }

    private async Task TryPublishIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class, IIntegrationEvent
    {
        try
        {
            await this.eventBusMessagePublisher.PublishAsync(integrationEvent, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            var outboxMessage = integrationEvent.ToFailedOutboxMessage();
            await this.outboxRepository.SaveAsync(outboxMessage, cancellationToken);
        }
    }
}

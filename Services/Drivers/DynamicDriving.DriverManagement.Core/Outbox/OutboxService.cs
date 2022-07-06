using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using MassTransit;

namespace DynamicDriving.DriverManagement.Core.Outbox;

public class OutboxService : IOutboxService
{
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IOutboxRepository outboxRepository;

    public OutboxService(IOutboxRepository outboxRepository, IPublishEndpoint publishEndpoint)
    {
        this.outboxRepository = outboxRepository;
        this.publishEndpoint = publishEndpoint;
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
            await this.publishEndpoint.Publish(integrationEvent, integrationEvent.GetType(), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is not OperationCanceledException)
            {
                throw;
            }

            var outboxMessage = integrationEvent.ToFailedOutboxMessage();
            await this.outboxRepository.SaveAsync(outboxMessage, cancellationToken).ConfigureAwait(false);
        }
    }
}

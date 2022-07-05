using DynamicDriving.EventBus;
using DynamicDriving.Events;
using MassTransit;

namespace DynamicDriving.MassTransit;

public class MassTransitEventPublisher : IEventBusMessagePublisher
{
    private readonly IPublishEndpoint publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        await this.publishEndpoint.Publish(integrationEvent, integrationEvent.GetType(), cancellationToken).ConfigureAwait(false);
    }
}

using DynamicDriving.EventBus;
using DynamicDriving.Events;
using MassTransit;
using RabbitMQ.Client;

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

    public async Task PublishAsync<T>(Guid messageId, string routingKey, T message)
        where T : class
    {
        await this.publishEndpoint.Publish(message, typeof(T), context =>
        {
            if (!context.TryGetPayload<RabbitMqSendContext>(out var rabbitMqSendContext))
            {
                throw new ArgumentException($"The current publish context does not have a {nameof(RabbitMqSendContext)} when trying to serialize {typeof(T).FullName}");
            }

            rabbitMqSendContext!.BasicProperties.Type = typeof(T).FullName;
            rabbitMqSendContext.BasicProperties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            context.MessageId = messageId;
            context.SetRoutingKey(routingKey);
        }).ConfigureAwait(false);
    }
}

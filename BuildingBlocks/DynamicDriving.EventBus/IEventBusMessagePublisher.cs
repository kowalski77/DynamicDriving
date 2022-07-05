using DynamicDriving.Events;

namespace DynamicDriving.EventBus;

public interface IEventBusMessagePublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    
    Task PublishAsync<T>(Guid messageId, string routingKey, T message) where T : class;
}

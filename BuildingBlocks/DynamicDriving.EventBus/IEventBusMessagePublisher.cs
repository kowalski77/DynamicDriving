using DynamicDriving.Events;

namespace DynamicDriving.EventBus;

public interface IEventBusMessagePublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}

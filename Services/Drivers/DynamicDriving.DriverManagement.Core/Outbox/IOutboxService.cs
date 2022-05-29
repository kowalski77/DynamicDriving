using DynamicDriving.Events;

namespace DynamicDriving.DriverManagement.Core.Outbox;

public interface IOutboxService
{
    Task PublishIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class, IIntegrationEvent;
}

using System.Text.Json;
using DynamicDriving.SharedKernel.Outbox;

namespace DynamicDriving.DriverManagement.Core.Outbox;

public static class OutboxMapper
{
    public static OutboxMessage ToFailedOutboxMessage<TIntegrationEvent>(this TIntegrationEvent integrationEvent)
        where TIntegrationEvent : class
    {
        var type = integrationEvent.GetType().FullName ??
                   throw new InvalidOperationException("The type of the message cannot be null.");

        var data = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
        var outboxMessage = new OutboxMessage(Guid.NewGuid(), DateTime.Now, type, data)
        {
            State = EventState.PublishedFailed
        };

        return outboxMessage;
    }
}

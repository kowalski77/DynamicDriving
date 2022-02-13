using System.Text.Json;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers;

public sealed class IntegrationEventSerializer : IIntegrationEventSerializer
{
    private readonly IEventContextFactory[] eventContextFactories;

    public IntegrationEventSerializer(IEventContextFactory[] eventContextFactories)
    {
        this.eventContextFactories = eventContextFactories ?? throw new ArgumentNullException(nameof(eventContextFactories));
    }

    public string Serialize(IIntegrationEvent integrationEvent)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        var integrationEventType = integrationEvent.GetType();
        var eventContextFactory = this.eventContextFactories.FirstOrDefault(x => x.ContextType == integrationEventType);
        if (eventContextFactory is null)
        {
            throw new InvalidOperationException($"There is no context factory register for type {integrationEventType}");
        }

        var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEventType, eventContextFactory.GetContext());

        return serializedIntegrationEvent;
    }
}

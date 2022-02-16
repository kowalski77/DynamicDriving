using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Serializers;

public sealed class IntegrationEventSerializer : IIntegrationEventSerializer
{
    private readonly IEventContextFactory[] eventContextFactories;

    public IntegrationEventSerializer(IEventContextFactory[] eventContextFactories)
    {
        this.eventContextFactories = eventContextFactories ??
                                     throw new ArgumentNullException(nameof(eventContextFactories));
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

    public async Task<T> DeserializeAsync<T>(Stream data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var eventContextFactory = this.eventContextFactories.FirstOrDefault(x => x.ContextType == typeof(T));
        if (eventContextFactory is null)
        {
            throw new InvalidOperationException($"There is no context factory register for type {typeof(T)}");
        }

        var message = await JsonSerializer.DeserializeAsync(data, (JsonTypeInfo<T>)eventContextFactory.GetJsonTypeInfo())
            .ConfigureAwait(false) ??
                      throw new SerializationException($"Could not deserialize type: {typeof(T)}");

        return message;
    }
}

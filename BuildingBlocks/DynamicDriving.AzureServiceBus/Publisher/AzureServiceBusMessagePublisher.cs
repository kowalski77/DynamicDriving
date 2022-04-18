using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.EventBus;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Publisher;

public sealed class AzureServiceBusMessagePublisher : IEventBusMessagePublisher, IAsyncDisposable
{
    private static readonly ConcurrentDictionary<Type, ServiceBusSender> ServiceBusSenders = new();

    private readonly IIntegrationEventSerializer integrationEventSerializer;
    private readonly ServiceBusClient serviceBusClient;

    public AzureServiceBusMessagePublisher(
        IIntegrationEventSerializer integrationEventSerializer,
        AzureServiceBusOptions options)
    {
        ArgumentNullException.ThrowIfNull(integrationEventSerializer);
        ArgumentNullException.ThrowIfNull(options);

        this.integrationEventSerializer = integrationEventSerializer;
        this.serviceBusClient = new ServiceBusClient(options.StorageConnectionString);
    }

    public async ValueTask DisposeAsync()
    {
        await this.serviceBusClient.DisposeAsync().ConfigureAwait(false);
        foreach (var serviceBusSender in ServiceBusSenders)
        {
            await serviceBusSender.Value.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        var integrationEventType = integrationEvent.GetType();
        var sender = ServiceBusSenders.GetOrAdd(integrationEventType, this.serviceBusClient.CreateSender(integrationEventType.Name));

        var serializedIntegrationEvent = this.integrationEventSerializer.Serialize(integrationEvent);
        await sender.SendMessageAsync(new ServiceBusMessage(serializedIntegrationEvent), cancellationToken).ConfigureAwait(false);
    }
}

using System.Collections.Concurrent;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using DynamicDriving.EventBus;
using DynamicDriving.Events;

namespace DynamicDriving.AzureServiceBus.Publisher;

public sealed class AzureServiceBusMessagePublisher : IEventBusMessagePublisher, IAsyncDisposable
{
    private static readonly ConcurrentDictionary<Type, ServiceBusSender> ServiceBusSenders = new();
    private readonly ServiceBusClient serviceBusClient;

    public AzureServiceBusMessagePublisher(AzureServiceBusOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

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

        var serializedIntegrationEvent = JsonSerializer.Serialize(integrationEvent, integrationEventType);
        await sender.SendMessageAsync(new ServiceBusMessage(serializedIntegrationEvent), cancellationToken).ConfigureAwait(false);
    }
}

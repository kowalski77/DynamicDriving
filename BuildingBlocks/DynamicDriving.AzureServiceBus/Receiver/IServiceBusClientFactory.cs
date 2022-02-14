using Azure.Messaging.ServiceBus;

namespace DynamicDriving.AzureServiceBus.Receiver;

public interface IServiceBusClientFactory
{
    ServiceBusClient Client { get; }
}

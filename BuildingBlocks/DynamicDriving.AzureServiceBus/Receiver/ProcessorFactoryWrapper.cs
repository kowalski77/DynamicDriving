using Azure.Messaging.ServiceBus;

namespace DynamicDriving.AzureServiceBus.Receiver;

public abstract class ProcessorFactoryWrapper
{
    public abstract ServiceBusProcessor CreateProcessor(string queue);
}

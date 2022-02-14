using DynamicDriving.AzureServiceBus.Receiver;

namespace DynamicDriving.AzureServiceBus;

public class AzureServiceBusOptions
{
    public string StorageConnectionString { get; set; } = default!;

    public IEnumerable<MessageProcessor> MessageProcessors { get; set; } = new List<MessageProcessor>();
}

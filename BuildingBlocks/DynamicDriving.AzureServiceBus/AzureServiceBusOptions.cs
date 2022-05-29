using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.EventBus.Serializers;

namespace DynamicDriving.AzureServiceBus;

public class AzureServiceBusOptions
{
    public string StorageConnectionString { get; set; } = default!;

    public IEnumerable<MessageProcessor> MessageProcessors { get; set; } = new List<MessageProcessor>();

    public IEnumerable<IEventContextFactory> EventContextFactories { get; set; } = new List<IEventContextFactory>();
}

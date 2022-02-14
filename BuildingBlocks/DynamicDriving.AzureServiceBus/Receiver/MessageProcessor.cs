namespace DynamicDriving.AzureServiceBus.Receiver;

public class MessageProcessor
{
    public MessageProcessor(string queue, Type type)
    {
        this.Queue = queue;
        this.Type = type;
    }

    public string Queue { get; }

    public Type Type { get; }
}

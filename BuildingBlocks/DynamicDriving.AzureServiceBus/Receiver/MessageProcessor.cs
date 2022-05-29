namespace DynamicDriving.AzureServiceBus.Receiver;

public class MessageProcessor
{
    public MessageProcessor(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        this.Queue = type.Name;
        this.Type = type;
    }

    public string Queue { get; }

    public Type Type { get; }
}

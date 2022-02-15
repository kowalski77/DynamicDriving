namespace DynamicDriving.AzureServiceBus.Receiver;

public interface IConsumer<in T>
{
    Task ExecuteAsync(T message);
}

namespace DynamicDriving.AzureServiceBus.Receiver;

public interface IMessageReceiver
{
    void AddProcessor(string queue, Type type);

    Task StartAsync();

    Task StopAsync();
}

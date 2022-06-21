namespace DynamicDriving.AzureServiceBus.Receiver;

public interface IMessageReceiver
{
    Task StartAsync();

    Task StopAsync();
}

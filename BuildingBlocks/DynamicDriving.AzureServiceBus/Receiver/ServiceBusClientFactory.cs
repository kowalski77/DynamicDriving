using Azure.Messaging.ServiceBus;

namespace DynamicDriving.AzureServiceBus.Receiver;

public sealed class ServiceBusClientFactory : IServiceBusClientFactory, IAsyncDisposable
{
    public ServiceBusClientFactory(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("The Azure Service Bus connection string cannot be empty");
        }

        this.Client = new ServiceBusClient(connectionString);
    }

    public ServiceBusClient Client { get; }

    public async ValueTask DisposeAsync()
    {
        await this.Client.DisposeAsync().ConfigureAwait(false);
    }
}

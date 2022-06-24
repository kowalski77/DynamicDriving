using Azure.Messaging.ServiceBus;

namespace DynamicDriving.AzureServiceBus.Receiver;

public sealed class MessageReceiver : IMessageReceiver, IAsyncDisposable
{
    private readonly List<ServiceBusProcessor> serviceBusProcessors = new();
    private readonly IServiceProvider serviceProvider;

    public MessageReceiver(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var processor in this.serviceBusProcessors)
        {
            await processor.DisposeAsync().ConfigureAwait(false);
        }
    }

    public void AddProcessorsForTypes(IEnumerable<Type> integrationEventTypes)
    {
        ArgumentNullException.ThrowIfNull(integrationEventTypes);

        foreach (var integrationEvenType in integrationEventTypes)
        {
            this.AddProcessor(integrationEvenType);
        }
    }

    private void AddProcessor(Type type)
    {
        var processorFactoryWrapper = this.GetProcessorFactoryWrapper(type);
        if (processorFactoryWrapper is null)
        {
            return;
        }

        this.serviceBusProcessors.Add(processorFactoryWrapper.CreateProcessor(type.Name));
    }

    public async Task StartAsync()
    {
        foreach (var processor in this.serviceBusProcessors)
        {
            await processor.StartProcessingAsync().ConfigureAwait(false);
        }
    }

    public async Task StopAsync()
    {
        foreach (var processor in this.serviceBusProcessors)
        {
            await processor.StopProcessingAsync().ConfigureAwait(false);
        }
    }

    private ProcessorFactoryWrapper? GetProcessorFactoryWrapper(Type type)
    {
        var processorFactoryWrapper = Activator.CreateInstance(
                typeof(ProcessorFactory<>).MakeGenericType(type),
                this.serviceProvider)
            as ProcessorFactoryWrapper;

        return processorFactoryWrapper;
    }
}

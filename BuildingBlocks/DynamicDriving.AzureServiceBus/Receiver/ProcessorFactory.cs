using Azure.Messaging.ServiceBus;
using DynamicDriving.EventBus.Serializers;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus.Receiver;

public sealed class ProcessorFactory<T> : ProcessorFactoryWrapper
{
    private readonly IServiceProvider serviceProvider;

    public ProcessorFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public override ServiceBusProcessor CreateProcessor(string queue)
    {
        var clientFactory = this.serviceProvider.GetRequiredService<IServiceBusClientFactory>();
        var processor = clientFactory.Client.CreateProcessor(queue);

        processor.ProcessMessageAsync += this.OnProcessMessageAsync;
        processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;

        return processor;
    }

    private async Task OnProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        using var scope = this.serviceProvider.CreateScope();

        var serializer = scope.ServiceProvider.GetRequiredService<IIntegrationEventSerializer>();
        var message = await serializer.DeserializeAsync<T>(arg.Message.Body.ToStream())
            .ConfigureAwait(false);

        var consumer = scope.ServiceProvider.GetRequiredService<IConsumer<T>>();

        await consumer.ExecuteAsync(message).ConfigureAwait(false);
    }

    private static Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        return Task.CompletedTask;
    }
}

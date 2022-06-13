﻿using System.Runtime.Serialization;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
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
        var message = await JsonSerializer.DeserializeAsync<T>(arg.Message.Body.ToStream()).ConfigureAwait(false) ?? 
                      throw new SerializationException($"Could not deserialize type of {typeof(T)}");

        using var scope = this.serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IConsumer<T>>();

        await consumer.ExecuteAsync(message).ConfigureAwait(false);
    }

    private static Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        return Task.CompletedTask;
    }
}

﻿// See https://aka.ms/new-console-template for more information

using System.Text.Json.Serialization;
using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.Events;

Console.WriteLine("Press a key to send a message through Azure Service Bus");
Console.ReadKey();

await using var serviceBus = new AzureServiceBusMessagePublisher(
    new IntegrationEventSerializer(new IEventContextFactory[] { new TestHarnessEventContextFactory() }),
    new AzureServiceBusOptions
    {
        StorageConnectionString = "Endpoint=sb://dynamic-driving.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Nrf+Eohs7r03wkddo9LHQf+pfyunP+WvBVe0PaotqOw="
    });

await serviceBus.PublishAsync(new TestHarnessEvent(Guid.NewGuid(), "1"));

public record TestHarnessEvent(Guid Id, string Value) : IIntegrationEvent;

public class TestHarnessEventContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TestHarnessEvent);

    public JsonSerializerContext GetContext() => TestHarnessEventContext.Default;
}

[JsonSerializable(typeof(TestHarnessEvent), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class TestHarnessEventContext : JsonSerializerContext
{
}

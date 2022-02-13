// See https://aka.ms/new-console-template for more information

using System.Text.Json.Serialization;
using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.Events;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.AddUserSecrets<Program>();
var config = builder.Build();

Console.WriteLine("Press a key to send a message through Azure Service Bus");
Console.ReadKey();

await using var serviceBus = new AzureServiceBusMessagePublisher(
    new IntegrationEventSerializer(new IEventContextFactory[] { new TestHarnessEventContextFactory() }),
    new AzureServiceBusOptions
    {
        StorageConnectionString = config["StorageConnectionString"]
    });

await serviceBus.PublishAsync(new TestHarnessEvent(Guid.NewGuid(), "value1"));

public record TestHarnessEvent(Guid Id, string Value) : IIntegrationEvent;

public class TestHarnessEventContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(TestHarnessEvent);

    public JsonSerializerContext GetContext() => TestHarnessEventContext.Default;
}

[JsonSerializable(typeof(TestHarnessEvent), GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class TestHarnessEventContext : JsonSerializerContext
{
}

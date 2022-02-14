// See https://aka.ms/new-console-template for more information

using System.Text.Json.Serialization;
using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.Events;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.AddUserSecrets<Program>();
var config = builder.Build();

Console.WriteLine("Press a key to send a message through Azure Service Bus");
Console.ReadKey();

await using var serviceBus = new AzureServiceBusMessagePublisher(
    new IntegrationEventSerializer(new IEventContextFactory[] { new PingContextFactory() }),
    new AzureServiceBusOptions
    {
        StorageConnectionString = config["StorageConnectionString"]
    });

await serviceBus.PublishAsync(new Ping(Guid.NewGuid(), 10));

public class PingContextFactory : IEventContextFactory
{
    public Type ContextType => typeof(Ping);

    public JsonSerializerContext GetContext() => PingContext.Default;
}

[JsonSerializable(typeof(Ping), GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class PingContext : JsonSerializerContext
{
}

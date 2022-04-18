// See https://aka.ms/new-console-template for more information

using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.EventBus.Serializers;
using DynamicDriving.EventBus.Serializers.Contexts;
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

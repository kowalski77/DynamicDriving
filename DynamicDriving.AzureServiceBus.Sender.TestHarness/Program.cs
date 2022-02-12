// See https://aka.ms/new-console-template for more information

using DynamicDriving.AzureServiceBus;
using DynamicDriving.Events;

Console.WriteLine("Press a key to send a message through Azure Service Bus");
Console.ReadKey();

await using var serviceBus = new AzureServiceBusMessagePublisher(new AzureServiceBusOptions
{
    StorageConnectionString = "Endpoint=sb://dynamic-driving.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Nrf+Eohs7r03wkddo9LHQf+pfyunP+WvBVe0PaotqOw="
});

await serviceBus.PublishAsync(new TestEvent(Guid.NewGuid(), "test 01", "1"));

public record TestEvent(Guid Id, string Name, string Value) : IIntegrationEvent;

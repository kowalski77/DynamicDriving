using DynamicDriving.Contracts.Events;
using DynamicDriving.MassTransit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("MassTransit.Sender.TestHarness... press a key to send a test message");
Console.ReadKey();

IServiceProvider serviceProvider;

ConfigureServices();

var pingMessage = new Ping(Guid.NewGuid(), 10);
var endpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();
await endpoint.Publish(pingMessage);

Console.ReadKey();
DisposeServices();

void ConfigureServices()
{
    IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

    var services = new ServiceCollection();
    services.AddSingleton(config);

    services.AddMassTransitWithRabbitMq();

    serviceProvider = services.BuildServiceProvider();
}

void DisposeServices()
{
    switch (serviceProvider)
    {
        case null:
            return;
        case IDisposable disposable:
            disposable.Dispose();
            break;
    }
}
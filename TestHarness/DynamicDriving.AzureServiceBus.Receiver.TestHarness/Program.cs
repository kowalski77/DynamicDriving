using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.Events;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddMediatR(typeof(PingNotification).Assembly);
builder.Services.AddScoped<ITranslator<Ping>, PingTranslator>();
builder.Services.AddAzureServiceBusReceiver(cfg =>
            {
                cfg.StorageConnectionString = builder.Configuration["StorageConnectionString"];
                cfg.MessageProcessors = new[]
                {
                    new MessageProcessor("ping", typeof(Ping))
                };
            });

var app = builder.Build();

app.Run();

public sealed record PingNotification(Guid Id, int Value) : INotification;

public sealed record PingNotificationHandler : INotificationHandler<PingNotification>
{
    public Task Handle(PingNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received ping notification: {notification}");

        return Task.CompletedTask;
    }
}

public sealed class PingTranslator : ITranslator<Ping>
{
    public INotification Translate(Ping message)
    {
        return new PingNotification(message.Id, message.Value);
    }
}

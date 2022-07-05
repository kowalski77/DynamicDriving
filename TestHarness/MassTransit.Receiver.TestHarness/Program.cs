using DynamicDriving.EventBus;
using DynamicDriving.Events;
using MassTransit.Receiver.TestHarness;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(PingNotification).Assembly);

builder.Services.AddTranslator<Ping, PingTranslator>();
builder.Services.AddEventBus();

var app = builder.Build();

app.Run();

public sealed record PingNotification(Guid Id, int Value) : INotification;

public sealed record TestMessageNotificationHandler : INotificationHandler<PingNotification>
{
    public Task Handle(PingNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received ping notification: {notification} with id: {notification.Id} and value: {notification.Value}");

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
using DynamicDriving.EventBus;
using DynamicDriving.Events;
using DynamicDriving.MassTransit;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(PingNotification).Assembly);
builder.Services.AddMassTransitWithRabbitMq();
builder.Services.AddTranslator<Ping, PingTranslator>();

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
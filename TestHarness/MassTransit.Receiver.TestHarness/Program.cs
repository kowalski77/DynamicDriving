using DynamicDriving.Events;
using DynamicDriving.MassTransit;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransitWithRabbitMq(typeof(PingConsumer).Assembly);

var app = builder.Build();

app.Run();

public sealed class PingConsumer : IConsumer<Ping>
{
    public Task Consume(ConsumeContext<Ping> context)
    {
        Console.WriteLine($"Received ping notification: {context.GetType()} with id: {context.Message.Id} and value: {context.Message.Value}");

        return Task.CompletedTask;
    }
}

public sealed class PingConsumer2 : IConsumer<Ping>
{
    public Task Consume(ConsumeContext<Ping> context)
    {
        Console.WriteLine($"Received ping notification: {context.GetType()} with id: {context.Message.Id} and value: {context.Message.Value}");

        return Task.CompletedTask;
    }
}
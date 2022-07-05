using DynamicDriving.Events;
using DynamicDriving.MassTransit;
using RabbitMQ.Client;

namespace MassTransit.Receiver.TestHarness;

public static class EventBusExtensions
{
    public static void AddEventBus(this IServiceCollection services)
    {
        services.AddMassTransitWithRabbitMq(configure =>
        {
            configure.ConfigureEndpoints = cfg => cfg.ConfigureAppEndpoints;
            configure.RegisterConsumerFor<Ping>();
        });
    }

    private static void ConfigureAppEndpoints(
        this IRabbitMqBusFactoryConfigurator cfg,
        IServiceProvider provider)
    {
        cfg.ConfigureContractsEndpoint<Ping>(provider);
    }

    private static void ConfigureContractsEndpoint<TType>(this IRabbitMqBusFactoryConfigurator cfg, IServiceProvider provider)
    where TType : class
    {
        cfg.ReceiveEndpoint(
            $"Receiver.TestHarness_{typeof(TType).FullName}",
            e =>
            {
                e.Bind("RTG_receive", x =>
                {
                    x.Durable = true;
                    x.ExchangeType = ExchangeType.Topic;
                    x.RoutingKey = $"TestHarness#{typeof(TType).FullName}";
                });

                e.Consumer<Consumer<TType>>(provider);
            });
    }
}

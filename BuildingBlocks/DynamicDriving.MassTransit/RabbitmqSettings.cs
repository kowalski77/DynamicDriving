using MassTransit;

namespace DynamicDriving.MassTransit;

public class RabbitmqSettings
{
    public string Host { get; init; } = default!;
    public string ServiceName { get; init; } = default!;

    public Func<IRabbitMqBusFactoryConfigurator, Action<IServiceProvider>>? ConfigureEndpoints { get; set; }

    public Action<IRetryConfigurator>? ConfigureRetries { get; set; }

    public ICollection<Type> ConsumerTypes { get; } = new List<Type>();

    public void RegisterConsumer<T>()
        where T : class
    {
        this.ConsumerTypes.Add(typeof(Consumer<T>));
    }
}

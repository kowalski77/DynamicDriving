using MassTransit;

namespace DynamicDriving.MassTransit;

public class MassTransitSettings
{
    public string RabbitMqHost { get; init; } = default!;
    
    public string ServiceName { get; init; } = default!;

    public Action<IRetryConfigurator>? ConfigureRetries { get; set; }
}

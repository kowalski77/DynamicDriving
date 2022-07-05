using DynamicDriving.EventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.MassTransit;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        Action<RabbitmqSettings>? configure = null)
    {
        var settings = new RabbitmqSettings();
        configure?.Invoke(settings);

        services.AddMassTransit(configure =>
        {
            foreach (var type in settings.ConsumerTypes)
            {
                configure.AddConsumer(type);
            }
            configure.UseRabbitMq(settings.ConfigureRetries);
        });

        services.AddScoped<IEventBusMessagePublisher, MassTransitEventPublisher>();

        return services;
    }

    private static void UseRabbitMq(
        this IBusRegistrationConfigurator configure,
        Action<IRetryConfigurator>? configureRetries)
    {
        configure.UsingRabbitMq((context, configurator) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSection(nameof(RabbitmqSettings)).Get<RabbitmqSettings>();

            configurator.Host(settings.Host);
            configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(settings.ServiceName, false));

            if (configureRetries is null)
            {
                configureRetries = (retryConfigurator) => retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
            }

            configurator.UseMessageRetry(configureRetries);
            configurator.UseInstrumentation(serviceName: settings.ServiceName);
        });
    }
}

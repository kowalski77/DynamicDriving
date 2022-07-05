using DynamicDriving.EventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.MassTransit;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        Action<MassTransitSettings>? configure = null)
    {
        var settings = new MassTransitSettings();
        configure?.Invoke(settings);

        services.AddMassTransit(configure =>
        {
            foreach (var type in settings.ConsumerTypes)
            {
                configure.AddConsumer(type);
            }
            configure.UseRabbitMq(settings.ConfigureRetries, settings.ConfigureEndpoints);
        });

        services.AddScoped<IEventBusMessagePublisher, MassTransitEventPublisher>();

        return services;
    }

    private static void UseRabbitMq(
        this IBusRegistrationConfigurator configure,
        Action<IRetryConfigurator>? configureRetries,
        Func<IRabbitMqBusFactoryConfigurator, Action<IServiceProvider>>? configureEndpoints)
    {
        configure.UsingRabbitMq((context, configurator) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>();

            configurator.Host(settings.RabbitMqHost);
            //configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(settings.ServiceName, false));

            if (configureRetries is null)
            {
                configureRetries = (retryConfigurator) => retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
            }
            
            if(configureEndpoints is not null)
            {
                configureEndpoints(configurator)(context);
            }

            configurator.UseServiceScope(context);
            configurator.UseMessageRetry(configureRetries);
            configurator.UseInstrumentation(serviceName: settings.ServiceName);
        });
    }
}

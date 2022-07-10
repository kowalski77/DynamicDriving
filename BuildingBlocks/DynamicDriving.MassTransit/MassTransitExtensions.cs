using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.MassTransit;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        Assembly? consumersAssembly = null,
        Action<MassTransitSettings>? configure = null)
    {
        var settings = new MassTransitSettings();
        configure?.Invoke(settings);

        services.AddMassTransit(configure =>
        {
            if(consumersAssembly is not null)
            {
                configure.AddConsumers(consumersAssembly);
            }
            configure.UsingDynamicDrivingRabbitMq(settings.ConfigureRetries);
        });

        return services;
    }

    public static void UsingDynamicDrivingRabbitMq(
        this IBusRegistrationConfigurator configure,
        Action<IRetryConfigurator>? configureRetries = null)
    {
        configure.UsingRabbitMq((context, configurator) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>();

            configurator.Host(settings.RabbitMqHost);
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

using System.Reflection;
using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus;

public static class AzureServiceBusExtensions
{
    public static IServiceCollection AddAzureServiceBusPublisher(this IServiceCollection services, Action<AzureServiceBusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var options = new AzureServiceBusOptions();
        configure.Invoke(options);

        services.AddSingleton<IEventBusMessagePublisher>(_ => 
            new AzureServiceBusMessagePublisher(new AzureServiceBusOptions
        {
            StorageConnectionString = options.StorageConnectionString
        }));

        return services;
    }

    public static IServiceCollection AddAzureServiceBusReceiver(this IServiceCollection services, Action<AzureServiceBusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var options = new AzureServiceBusOptions();
        configure.Invoke(options);

        services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>(_ => new ServiceBusClientFactory(options.StorageConnectionString));

        services.AddSingleton<IMessageReceiver>(sp =>
        {
            var messageReceiverEndPoint = new MessageReceiver(sp);
            messageReceiverEndPoint.AddProcessorsForAssembly(options.IntegrationEventsAssembly);

            return messageReceiverEndPoint;
        });

        services.AddScoped(typeof(IConsumer<>), typeof(Consumer<>));

        services.AddHostedService<ServiceBusReceiverHostedService>();

        return services;
    }

    public static IServiceCollection AddTranslator<TEvent, TTranslator>(this IServiceCollection services)
        where TTranslator : class, ITranslator<TEvent>
    {
        services.AddScoped<ITranslator<TEvent>, TTranslator>();

        return services;
    }
}

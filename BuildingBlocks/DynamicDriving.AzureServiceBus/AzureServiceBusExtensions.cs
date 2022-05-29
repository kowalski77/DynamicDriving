﻿using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.EventBus;
using DynamicDriving.EventBus.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus;

public static class AzureServiceBusExtensions
{
    public static IServiceCollection AddAzureServiceBusPublisher(this IServiceCollection services, IEventContextFactory[] eventContextFactories, IConfiguration configuration)
    {
        services.AddSingleton<IEventBusMessagePublisher>(_ => new AzureServiceBusMessagePublisher(new IntegrationEventSerializer(eventContextFactories), new AzureServiceBusOptions
        {
            StorageConnectionString = configuration["StorageConnectionString"]
        }));

        return services;
    }

    public static IServiceCollection AddAzureServiceBusReceiver(this IServiceCollection services, Action<AzureServiceBusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var options = new AzureServiceBusOptions();
        configure.Invoke(options);

        services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>(_ => new ServiceBusClientFactory(options.StorageConnectionString));
        services.AddScoped<IIntegrationEventSerializer, IntegrationEventSerializer>(_ => new IntegrationEventSerializer(options.EventContextFactories));

        services.AddSingleton<IMessageReceiver>(sp =>
        {
            var messageReceiverEndPoint = new MessageReceiver(sp);
            foreach (var processorRegistration in options.MessageProcessors)
            {
                messageReceiverEndPoint.AddProcessor(processorRegistration.Queue, processorRegistration.Type);
            }

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

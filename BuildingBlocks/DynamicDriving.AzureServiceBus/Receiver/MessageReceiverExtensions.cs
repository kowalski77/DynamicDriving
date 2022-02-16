﻿using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.AzureServiceBus.Serializers.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus.Receiver;

public static class MessageReceiverExtensions
{
    public static IServiceCollection AddAzureServiceBusReceiver(this IServiceCollection services, Action<AzureServiceBusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var options = new AzureServiceBusOptions();
        configure.Invoke(options);

        services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>(_ => new ServiceBusClientFactory(options.StorageConnectionString));
        services.AddScoped(typeof(IConsumer<>), typeof(Consumer<>));
        services.AddScoped<IIntegrationEventSerializer, IntegrationEventSerializer>(_ => new IntegrationEventSerializer(new IEventContextFactory[]
        {
            new PingContextFactory()
        }));

        services.AddSingleton(sp =>
        {
            var messageReceiverEndPoint = new MessageReceiver(sp);
            foreach (var processorRegistration in options.MessageProcessors)
            {
                messageReceiverEndPoint.AddProcessor(processorRegistration.Queue, processorRegistration.Type);
            }

            return messageReceiverEndPoint;
        });

        services.AddHostedService<ServiceBusReceiverHostedService>();

        return services;
    }

    public static IServiceCollection AddTranslator<TEvent, TTranslator>(this IServiceCollection services)
        where TTranslator : class, ITranslator<TEvent>
    {
        services.AddSingleton<ITranslator<TEvent>, TTranslator>();

        return services;
    }
}

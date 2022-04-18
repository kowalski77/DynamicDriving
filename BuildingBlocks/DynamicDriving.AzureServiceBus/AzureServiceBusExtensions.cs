using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.EventBus;
using DynamicDriving.EventBus.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus;

public static class AzureServiceBusExtensions
{
    // TODO: refactor
    public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IEventContextFactory[] eventContextFactories, IConfiguration configuration)
    {
        services.AddSingleton<IEventBusMessagePublisher>(_ => new AzureServiceBusMessagePublisher(new IntegrationEventSerializer(eventContextFactories), new AzureServiceBusOptions
        {
            StorageConnectionString = configuration["StorageConnectionString"]
        }));

        return services;
    }
}

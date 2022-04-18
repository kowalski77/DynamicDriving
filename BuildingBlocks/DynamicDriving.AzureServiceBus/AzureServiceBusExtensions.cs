using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.AzureServiceBus.Serializers.Contexts;
using DynamicDriving.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.AzureServiceBus;

public static class AzureServiceBusExtensions
{
    public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBusMessagePublisher>(_ => new AzureServiceBusMessagePublisher(new IntegrationEventSerializer(new IEventContextFactory[]
        {
            new TripConfirmedContextFactory()
        }), new AzureServiceBusOptions
        {
            StorageConnectionString = configuration["StorageConnectionString"]
        }));

        return services;
    }
}

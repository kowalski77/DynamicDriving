using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Publisher;
using DynamicDriving.AzureServiceBus.Serializers;
using DynamicDriving.AzureServiceBus.Serializers.Contexts;
using DynamicDriving.EventBus;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Outbox;
using DynamicDriving.TripManagement.Application.Behaviors;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]
namespace DynamicDriving.TripManagement.Application;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    { 
        ArgumentNullException.ThrowIfNull(services);

        services.AddMediatR(typeof(CreateDraftTripHandler).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        services.AddScoped<IOutboxService>(sp => new OutboxService(
            sp.GetRequiredService<IDbContext>(),
            dc => new OutboxRepository(dc)));

        services.AddSingleton<IEventBusMessagePublisher>(_ => new AzureServiceBusMessagePublisher(new IntegrationEventSerializer(new IEventContextFactory[]
        {
            new TripConfirmedContextFactory()
        }), new AzureServiceBusOptions
        {
            StorageConnectionString = configuration["StorageConnectionString"]
        }));
    }
}

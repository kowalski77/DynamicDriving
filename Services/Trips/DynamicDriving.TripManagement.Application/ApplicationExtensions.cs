using DynamicDriving.AzureServiceBus;
using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.EventBus;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Outbox.Sql;
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
            sp.GetRequiredService<IEventBusMessagePublisher>(),
            dc => new OutboxRepository(dc)));

        services.AddAzureServiceBusPublisher(configure =>
        {
            configure.StorageConnectionString = configuration["StorageConnectionString"];
        });

        services.AddAzureServiceBusReceiver(configure =>
        {
            configure.StorageConnectionString = configuration["StorageConnectionString"];
            configure.IntegrationEventsAssembly = typeof(DriverCreated).Assembly;
        });
    }
}

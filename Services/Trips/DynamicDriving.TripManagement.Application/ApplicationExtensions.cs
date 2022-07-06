using DynamicDriving.MassTransit;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Application.Behaviors;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MassTransit;
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

        services.AddMassTransitWithRabbitMq();

        services.AddScoped<IOutboxService>(sp => new OutboxService(
            sp.GetRequiredService<IDbContext>(),
            sp.GetRequiredService<IPublishEndpoint>(),
            dc => new OutboxRepository(dc)));
    }
}

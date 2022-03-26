using DynamicDriving.TripManagement.Application.Trips.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]
namespace DynamicDriving.TripManagement.Application;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    { 
        ArgumentNullException.ThrowIfNull(services);

        services.AddMediatR(typeof(CreateDraftTripHandler).Assembly);
        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
    }
}

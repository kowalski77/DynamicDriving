using DynamicDriving.DriverManagement.Core.Trips;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.DriverManagement.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITripsRepository, TripsRepository>();

        return services;
    }
}

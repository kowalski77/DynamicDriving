using DynamicDriving.TripManagement.Domain.LocationsAggregate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public static class AgentsExtensions
{
    public static void AddAgents(this IServiceCollection services)
    {
        services.AddScoped<ILocationProvider, LocationProvider>();
    }
}

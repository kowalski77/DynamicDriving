using DynamicDriving.TripManagement.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.TripManagement.Infrastructure.Agents;

public static class AgentsExtensions
{
    public static void AddAgents(this IServiceCollection services)
    {
        services.AddScoped<ICoordinatesAgent, FakeCoordinatesAgent>();
    }
}

using DynamicDriving.TripManagement.Infrastructure.Agents;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.TripManagement.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAgents();
        services.AddRepositories();
        services.AddSqlPersistence(configuration.GetConnectionString("DefaultConnection"));
    }
}

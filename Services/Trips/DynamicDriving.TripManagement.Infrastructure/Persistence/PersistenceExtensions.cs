using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public static class PersistenceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<ITripReadRepository, TripReadRepository>();
    }

    public static void AddSqlPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TripManagementContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });
        });
    }
}

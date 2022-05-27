using System.Reflection;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Outbox;
using DynamicDriving.SharedKernel.Outbox.Sql;
using DynamicDriving.TripManagement.Domain.CitiesAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public static class PersistenceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<ITripReadRepository, TripReadRepository>();

        services.AddScoped<ICityRepository, CityRepository>();
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

        services.AddDbContext<OutboxContext>(options =>
            options.UseSqlServer(connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TripManagementContext).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                }));

        services.AddScoped<IDbContext, TripManagementContext>();
    }
}

using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicDriving.DriverManagement.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Guards.ThrowIfNull(configuration);

        var mongoOptions = configuration.GetValue<MongoOptions>(nameof(MongoOptions));
        services.AddMongo(mongoOptions);
        services.AddScoped<IMongoRepository<Trip>, MongoRepository<Trip>>();

        return services;
    }
}

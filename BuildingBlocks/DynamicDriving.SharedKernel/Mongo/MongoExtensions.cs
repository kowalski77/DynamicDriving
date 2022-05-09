using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public static class MongoExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, MongoOptions options)
    {
        Guards.ThrowIfNull(options);

        var client = new MongoClient(options.Client);
        var database = client.GetDatabase(options.Database);

        services.AddSingleton(database);

        return services;
    }
}

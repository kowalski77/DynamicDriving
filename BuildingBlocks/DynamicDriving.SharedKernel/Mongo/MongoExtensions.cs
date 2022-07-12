using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public static class MongoExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var mongoOptions = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();

            var client = new MongoClient(mongoOptions.Client);
            var database = client.GetDatabase(mongoOptions.Database);

            return database;
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services)
        where T : class, IEntity
    {
        services.AddScoped<IMongoRepository<T>, MongoRepository<T>>();

        return services;
    }
}

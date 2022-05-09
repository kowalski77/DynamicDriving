using System.Linq.Expressions;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public class MongoRepository<T> : IMongoRepository<T>
    where T : class, IEntity
{
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        Guards.ThrowIfNull(database);

        this.dbCollection = database.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await this.dbCollection.Find(this.filterBuilder.Empty).ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await this.dbCollection.Find(filter).ToListAsync().ConfigureAwait(false);
    }

    public async Task<T> GetAsync(Guid id)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        return await this.dbCollection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await this.dbCollection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task CreateAsync(T entity)
    {
        Guards.ThrowIfNull(entity);

        await this.dbCollection.InsertOneAsync(entity).ConfigureAwait(false);
    }

    public async Task UpdateAsync(T entity)
    {
        Guards.ThrowIfNull(entity);

        var filter = this.filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await this.dbCollection.ReplaceOneAsync(filter, entity).ConfigureAwait(false);
    }

    public async Task RemoveAsync(Guid id)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        await this.dbCollection.DeleteOneAsync(filter).ConfigureAwait(false);
    }
}

using System.Linq.Expressions;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public class MongoRepository<T> : IMongoRepository<T>
    where T : class, IEntity
{
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database)
    {
        Guards.ThrowIfNull(database);

        this.dbCollection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.dbCollection.Find(this.filterBuilder.Empty).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await this.dbCollection.Find(filter).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        return await this.dbCollection.Find(filter).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await this.dbCollection.Find(filter).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(entity);

        try
        {
            await this.dbCollection.InsertOneAsync(entity, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        catch (MongoWriteException e)
        {
            if (e.InnerException is MongoBulkWriteException && 
                e.InnerException.Message.Contains("DuplicateKey", StringComparison.OrdinalIgnoreCase))
            {
                throw new DuplicateIdException($"{typeof(T).Name} with id: {entity.Id} already exists");
            }

            throw;
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(entity);

        var filter = this.filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await this.dbCollection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        await this.dbCollection.DeleteOneAsync(filter, cancellationToken).ConfigureAwait(false);
    }
}

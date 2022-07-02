using System.Linq.Expressions;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public abstract class MongoRepository<T> : IMongoRepository<T>
    where T : class, IEntity
{
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    protected MongoRepository(IMongoDatabase database)
    {
        Guards.ThrowIfNull(database);

        this.Collection = database.GetCollection<T>(typeof(T).Name);
    }

    protected IMongoCollection<T> Collection { get; }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.Collection.Find(this.filterBuilder.Empty).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await this.Collection.Find(filter).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TR>> GetAllAsync<TR>(Expression<Func<T, bool>> filter, FindOptions<T, TR> findOptions, CancellationToken cancellationToken = default)
    {
        return await (await this.Collection.FindAsync(filter, findOptions, cancellationToken).ConfigureAwait(false)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        return await this.Collection.Find(filter).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await this.Collection.Find(filter).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(entity);

        await this.Collection.InsertOneAsync(entity, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(entity);

        var filter = this.filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await this.Collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = this.filterBuilder.Eq(entity => entity.Id, id);
        await this.Collection.DeleteOneAsync(filter, cancellationToken).ConfigureAwait(false);
    }
}

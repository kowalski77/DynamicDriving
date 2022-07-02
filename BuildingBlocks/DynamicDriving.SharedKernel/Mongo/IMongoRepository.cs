using System.Linq.Expressions;
using MongoDB.Driver;

namespace DynamicDriving.SharedKernel.Mongo;

public interface IMongoRepository<T> where T : class, IEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TR>> GetAllAsync<TR>(Expression<Func<T, bool>> filter, FindOptions<T, TR> findOptions, CancellationToken cancellationToken = default);
}

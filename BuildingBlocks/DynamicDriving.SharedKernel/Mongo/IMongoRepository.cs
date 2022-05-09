using System.Linq.Expressions;

namespace DynamicDriving.SharedKernel.Mongo;

public interface IMongoRepository<T> where T : class, IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}

using System.Linq.Expressions;

namespace DynamicDriving.SharedKernel.Mongo;

// TODO: extract to shared kernel
public interface IMongoRepository<T> where T : class, IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();

    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);

    Task<T> GetAsync(Guid id);

    Task<T> GetAsync(Expression<Func<T, bool>> filter);

    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task RemoveAsync(Guid id);
}

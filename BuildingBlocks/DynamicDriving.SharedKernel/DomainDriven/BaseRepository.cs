namespace DynamicDriving.SharedKernel.DomainDriven;

public class BaseRepository<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    public BaseRepository(TransactionContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected TransactionContext Context { get; }

    public IUnitOfWork UnitOfWork => this.Context;

    public virtual T Add(T item)
    {
        return this.Context.Add(item).Entity;
    }

    public virtual async Task<Maybe<T>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.Context.FindAsync<T>(new object[] { id }, cancellationToken).ConfigureAwait(false);
    }
}

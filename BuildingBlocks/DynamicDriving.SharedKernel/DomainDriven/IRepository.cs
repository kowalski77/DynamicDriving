namespace DynamicDriving.SharedKernel.DomainDriven;

public interface IRepository<T>
    where T : class, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

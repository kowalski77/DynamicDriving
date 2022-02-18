namespace DynamicDriving.SharedKernel.DomainDriven;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
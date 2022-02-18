namespace DynamicDriving.SharedKernel.Repositories;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
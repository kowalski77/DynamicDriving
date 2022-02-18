using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamicDriving.SharedKernel.Repositories;

public interface IDbContext
{
    DatabaseFacade DatabaseFacade { get; }

    IDbContextTransaction GetCurrentTransaction();

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
}

using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamicDriving.SharedKernel.DomainDriven;

public abstract class TransactionContext : DbContext, IDbContext, IUnitOfWork
{
    private readonly IMediator mediator;
    private IDbContextTransaction? currentTransaction;

    protected TransactionContext(
        DbContextOptions options,
        IMediator mediator)
        : base(options)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public DatabaseFacade DatabaseFacade => base.Database;

    public IDbContextTransaction GetCurrentTransaction()
    {
        if (this.currentTransaction is null)
        {
            throw new InvalidOperationException("Current transaction is null");
        }

        return this.currentTransaction;
    }

    public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (this.currentTransaction is not null)
        {
            throw new InvalidOperationException("There is already a transaction in progress.");
        }

        this.currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken)
            .ConfigureAwait(false);

        return this.currentTransaction;
    }

    public virtual Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction != this.currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        return this.TryCommitTransactionAsync(transaction, cancellationToken);
    }

    public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await this.mediator.PublishDomainEventsAsync(this, cancellationToken).ConfigureAwait(false);

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    private async Task TryCommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            this.RollbackTransaction();
            throw;
        }
        finally
        {
            if (this.currentTransaction is not null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            this.currentTransaction?.Rollback();
        }
        finally
        {
            if (this.currentTransaction is not null)
            {
                this.currentTransaction.Dispose();
                this.currentTransaction = null;
            }
        }
    }
}

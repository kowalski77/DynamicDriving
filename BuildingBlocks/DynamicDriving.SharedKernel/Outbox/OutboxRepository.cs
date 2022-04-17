using System.Data.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DynamicDriving.SharedKernel.Outbox;

public sealed class OutboxRepository : IOutboxRepository, IDisposable
{
    private readonly OutboxContext context;

    public OutboxRepository(DbConnection dbConnection)
    {
        if (dbConnection == null)
        {
            throw new ArgumentNullException(nameof(dbConnection));
        }

        this.context = new OutboxContext(new DbContextOptionsBuilder<OutboxContext>()
            .UseSqlServer(dbConnection).Options);
    }

    public async Task SaveMessageAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);
        ArgumentNullException.ThrowIfNull(transaction);

        await this.context.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken).ConfigureAwait(false);

        var outboxMessage = GetOutboxMessage(integrationEvent, transaction.TransactionId);
        await this.context.AddAsync(outboxMessage, cancellationToken).ConfigureAwait(false);

        await this.context.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkMessageAsInProgressAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        await this.UpdateStatusAsync(messageId, EventState.InProgress, cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkMessageAsPublishedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        await this.UpdateStatusAsync(messageId, EventState.Published, cancellationToken).ConfigureAwait(false);
    }

    public async Task MarkMessageAsFailedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        await this.UpdateStatusAsync(messageId, EventState.PublishedFailed, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var outboxMessages = await (this.context.OutboxMessages!)
            .Where(e => e.Id == transactionId && e.State == EventState.NotPublished)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return outboxMessages;
    }

    public async Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(CancellationToken cancellationToken = default)
    {
        var outboxMessages = await (this.context.OutboxMessages!)
            .Where(e => e.State == EventState.NotPublished || e.State == EventState.PublishedFailed)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return outboxMessages;
    }

    private async Task UpdateStatusAsync(Guid messageId, EventState eventState, CancellationToken cancellationToken = default)
    {
        var message = this.context.OutboxMessages!.Single(x => x.Id == messageId);
        message.State = eventState;

        this.context.OutboxMessages!.Update(message);

        await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static OutboxMessage GetOutboxMessage<TIntegrationEvent>(TIntegrationEvent integrationEvent, Guid transactionId)
        where TIntegrationEvent : class
    {
        var type = integrationEvent.GetType().FullName ??
                   throw new InvalidOperationException("The type of the message cannot be null.");

        var data = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
        var outboxMessage = new OutboxMessage(transactionId, DateTime.Now, type, data);

        return outboxMessage;
    }

    public void Dispose()
    {
        this.context.Dispose();
    }
}

using Microsoft.EntityFrameworkCore.Storage;

namespace DynamicDriving.SharedKernel.Outbox;

public interface IOutboxRepository
{
    Task SaveMessageAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class;

    Task MarkMessageAsInProgressAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task MarkMessageAsPublishedAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task MarkMessageAsFailedAsync(Guid messageId, CancellationToken cancellationToken = default);

    Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(Guid transactionId, CancellationToken cancellationToken = default);

    Task<Maybe<IReadOnlyList<OutboxMessage>>> GetNotPublishedAsync(CancellationToken cancellationToken = default);
}

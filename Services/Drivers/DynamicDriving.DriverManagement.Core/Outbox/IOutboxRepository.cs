using DynamicDriving.SharedKernel.Outbox;

namespace DynamicDriving.DriverManagement.Core.Outbox;

public interface IOutboxRepository
{
    Task SaveAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default);
}

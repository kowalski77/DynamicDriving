using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Outbox;
using MongoDB.Driver;

namespace DynamicDriving.DriverManagement.Infrastructure;

public class OutboxRepository : IOutboxRepository
{
    private readonly IMongoCollection<OutboxMessage> outboxMessagesCollection;

    public OutboxRepository(IMongoDatabase database)
    {
        Guards.ThrowIfNull(database);

        this.outboxMessagesCollection = database.GetCollection<OutboxMessage>(nameof(OutboxMessage));
    }

    public async Task SaveAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(outboxMessage);

        await this.outboxMessagesCollection.InsertOneAsync(outboxMessage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

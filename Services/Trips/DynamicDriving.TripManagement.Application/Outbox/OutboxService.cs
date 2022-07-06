using System.Data.Common;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.SharedKernel.Outbox;
using DynamicDriving.SharedKernel.Outbox.Sql;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Application.Outbox;

public sealed class OutboxService : IOutboxService
{
    private readonly IDbContext context;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IOutboxRepository outboxRepository;

    public OutboxService(IDbContext context, IPublishEndpoint publishEndpoint, Func<DbConnection, IOutboxRepository> outboxRepositoryFactory)
    {
        Guards.ThrowIfNull(context);
        Guards.ThrowIfNull(publishEndpoint);
        Guards.ThrowIfNull(outboxRepositoryFactory);

        this.context = context;
        this.publishEndpoint = publishEndpoint;
        this.outboxRepository = outboxRepositoryFactory(context.DatabaseFacade.GetDbConnection());
    }

    public async Task AddIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : class, IIntegrationEvent
    {
        Guards.ThrowIfNull(integrationEvent);

        await this.outboxRepository.SaveMessageAsync(integrationEvent, this.context.GetCurrentTransaction(), cancellationToken).ConfigureAwait(false);
    }

    public async Task PublishIntegrationEventsAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var pendingOutboxMessages = await this.outboxRepository.GetNotPublishedAsync(transactionId, cancellationToken).ConfigureAwait(false);
        if (pendingOutboxMessages.HasValue)
        {
            foreach (var outboxMessage in pendingOutboxMessages.Value)
            {
                await this.TryPublishIntegrationEventsAsync(outboxMessage, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public async Task PublishPendingIntegrationEventsAsync(CancellationToken cancellationToken = default)
    {
        var pendingOutboxMessages = await this.outboxRepository.GetNotPublishedAsync(cancellationToken).ConfigureAwait(false);
        if (pendingOutboxMessages.HasValue)
        {
            foreach (var outboxMessage in pendingOutboxMessages.Value)
            {
                await this.TryPublishIntegrationEventsAsync(outboxMessage, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private async Task TryPublishIntegrationEventsAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = await OutboxSerializer.DeserializeAsync<IIntegrationEvent>(outboxMessage);

            await this.publishEndpoint.Publish(message, message.GetType(), cancellationToken).ConfigureAwait(false);

            await this.outboxRepository.MarkMessageAsPublishedAsync(outboxMessage.Id, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await this.outboxRepository.MarkMessageAsFailedAsync(outboxMessage.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}

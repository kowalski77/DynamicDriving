using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Outbox;

namespace DynamicDriving.TripManagement.API.HostedServices;

public sealed class OutboxHostedService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private Task? executingTask;
    private Timer timer = default!;

    public OutboxHostedService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = Guards.ThrowIfNull(serviceProvider);
    }

    public override void Dispose()
    {
        this.Dispose(true);
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.timer = new Timer(o => this.ExecuteInternalTask(o, stoppingToken), null, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1));

        return Task.CompletedTask;
    }

    private void ExecuteInternalTask(object? _, CancellationToken cancellationToken)
    {
        this.timer.Change(Timeout.Infinite, 0);
        this.executingTask = this.ExecuteOutboxTaskAsync(cancellationToken);
    }

    private async Task ExecuteOutboxTaskAsync(CancellationToken cancellationToken)
    {
        using (var scope = this.serviceProvider.CreateScope())
        {
            var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
            await outboxService.PublishPendingIntegrationEventsAsync(cancellationToken).ConfigureAwait(false);
        }

        this.timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1));
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        this.executingTask?.Dispose();
        this.timer.Dispose();
    }
}

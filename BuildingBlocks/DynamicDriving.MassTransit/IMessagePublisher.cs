namespace DynamicDriving.MassTransit;

public interface IMessagePublisher
{
    Task PublishAsync(object message, Type type, CancellationToken cancellationToken = default);
}

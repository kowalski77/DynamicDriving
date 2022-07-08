using MassTransit;

namespace DynamicDriving.MassTransit;

public class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint publishEndpoint;

    public MessagePublisher(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync(object message, Type type, CancellationToken cancellationToken = default)
    {
        return this.publishEndpoint.Publish(message, type, cancellationToken);
    }
}

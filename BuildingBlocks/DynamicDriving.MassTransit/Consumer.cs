using DynamicDriving.EventBus;
using MassTransit;
using MediatR;

namespace DynamicDriving.MassTransit;

public sealed class Consumer<T> : IConsumer<T>
    where T : class
{
    private readonly IMediator mediator;
    private readonly ITranslator<T> translator;

    public Consumer(IMediator mediator, ITranslator<T> translator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.translator = translator ?? throw new ArgumentNullException(nameof(translator));
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var notification = this.translator.Translate(context.Message);

        await this.mediator.Publish(notification).ConfigureAwait(false);
    }
}

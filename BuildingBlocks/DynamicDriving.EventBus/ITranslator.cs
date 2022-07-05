using MediatR;

namespace DynamicDriving.EventBus;

public interface ITranslator<in T>
{
    INotification Translate(T message);
}

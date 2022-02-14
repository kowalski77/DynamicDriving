using MediatR;

namespace DynamicDriving.AzureServiceBus.Receiver;

public interface ITranslator<in T>
{
    INotification Translate(T message);
}

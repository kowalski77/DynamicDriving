using MediatR;

namespace DynamicDriving.SharedKernel.Mediator;

public interface IDomainNotificationHandler<in T> : INotificationHandler<T>
    where T : IDomainNotification
{
}

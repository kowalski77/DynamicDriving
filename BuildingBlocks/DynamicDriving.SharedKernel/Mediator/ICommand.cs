using MediatR;

namespace DynamicDriving.SharedKernel.Mediator;

public interface ICommand<out TCommand> : IRequest<TCommand>
{
}

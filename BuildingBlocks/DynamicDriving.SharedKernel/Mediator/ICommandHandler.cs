using MediatR;

namespace DynamicDriving.SharedKernel.Mediator;

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : ICommand<TResponse>
{
}

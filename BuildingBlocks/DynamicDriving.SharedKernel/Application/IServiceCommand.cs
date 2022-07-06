namespace DynamicDriving.SharedKernel.Application;

public interface IServiceCommand<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}

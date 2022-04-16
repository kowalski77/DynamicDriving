namespace DynamicDriving.SharedKernel.Results;

public static class ResultExtensions
{
    public static Result<T> Do<T>(this Result _, Func<Result<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return func();
    }

    public static async Task<Result<T>> Do<T>(this Result _, Func<Task<Result<T>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return await func().ConfigureAwait(false);
    }

    public static Result Validate(this Result _, params Result[] results)
    {
        var errorCollection = (from result in results
                               where result.Failure
                               select result.Error!)
            .ToList();

        return errorCollection.Any() ?
            errorCollection.First()! :  //TODO: rethink this, handle error collections in result & envelope
            Result.Ok();
    }

    public static async Task<Result> OnSuccess(this Result result, Func<Task<Result>> func)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(func);

        return result.Success ? 
            await func().ConfigureAwait(false) 
            : result.Error!;
    }

    public static async Task<Result<(T, TK)>> OnSuccess<T,TK>(this Task<Result> result, Func<Task<Result<(T, TK)>>> func)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(func);

        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Success ? 
            await func().ConfigureAwait(false) : 
            awaitedResult.Error!;
    }

    public static async Task<Result<T>> OnSuccess<T>(this Result result, Func<Task<Result<T>>> func)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(func);

        return result.Success ? 
            await func().ConfigureAwait(false) 
            : result.Error!;
    }

    public static async Task<Result<TR>> OnSuccess<T, TR>(this Task<Result<T>> result, Func<T, TR> mapper)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Success ? 
            mapper(awaitedResult.Value) : 
            awaitedResult.Error!;
    }

    public static async Task<Result> OnSuccess<T>(this Task<Result<T>> result, Func<T, Task<Result>> func)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(func);

        var awaitedResult = await result.ConfigureAwait(false);

        return awaitedResult.Success ?
            await func(awaitedResult.Value).ConfigureAwait(false) : 
            awaitedResult.Error!;
    }
}

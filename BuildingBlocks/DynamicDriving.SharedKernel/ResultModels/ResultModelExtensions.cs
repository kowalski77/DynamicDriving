using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.SharedKernel.ResultModels;

public static class ResultModelExtensions
{
    public static IResultModel Validate(params Result[] results)
    {
        var errorCollection = (from result in results
                where result.Failure
                select Result.Fail(result.Error!))
            .ToList();

        return errorCollection.Any() ? 
            ResultModel.Fail(errorCollection.Select(x => x.Error!)) : 
            ResultModel.Ok();
    }

    public static async Task<IResultModel<T>> OnSuccess<T>(this IResultModel resultModel, Func<Task<IResultModel<T>>> func)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return resultModel.Success ? await func().ConfigureAwait(false) : ResultModel.Fail<T>(resultModel.ErrorResult);
    }

    public static async Task<IResultModel> OnSuccess<T>(this Task<IResultModel<T>> resultModel, Func<T, IResultModel> func)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        var awaitedResultModel = await resultModel.ConfigureAwait(false);

        return awaitedResultModel.Success ? func(awaitedResultModel.Value) : ResultModel.Fail(awaitedResultModel.ErrorResult);
    }

    public static async Task<IResultModel> OnSuccess(this Task<IResultModel> resultModel, Func<Task> action)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var awaitedResultModel = await resultModel.ConfigureAwait(false);
        if (!awaitedResultModel.Success)
        {
            return ResultModel.Fail(awaitedResultModel.ErrorResult);
        }

        await action().ConfigureAwait(false);

        return ResultModel.Ok();
    }

    public static async Task<IResultModel<TR>> OnSuccess<T, TR>(this Task<IResultModel<T>> resultModel, Func<T, IResultModel<TR>> func)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        var awaitedResultModel = await resultModel.ConfigureAwait(false);

        return awaitedResultModel.Success ? func(awaitedResultModel.Value) : ResultModel.Fail<TR>(awaitedResultModel.ErrorResult);
    }

    public static async Task<IResultModel> OnSuccess<T>(this Task<IResultModel<T>> resultModel, Func<T, Task<IResultModel>> func)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        var awaitedResultModel = await resultModel.ConfigureAwait(false);

        return awaitedResultModel.Success ? await func(awaitedResultModel.Value).ConfigureAwait(false) : ResultModel.Fail(awaitedResultModel.ErrorResult);
    }

    public static async Task<IResultModel<TR>> OnSuccess<T, TR>(this Task<IResultModel<T>> resultModel, Func<T, Task<IResultModel<TR>>> func)
    {
        if (resultModel == null)
        {
            throw new ArgumentNullException(nameof(resultModel));
        }

        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        var awaitedResultModel = await resultModel.ConfigureAwait(false);

        return awaitedResultModel.Success ? await func(awaitedResultModel.Value).ConfigureAwait(false) : ResultModel.Fail<TR>(awaitedResultModel.ErrorResult);
    }
}

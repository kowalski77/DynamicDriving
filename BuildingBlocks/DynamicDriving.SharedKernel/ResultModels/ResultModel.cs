using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.SharedKernel.ResultModels;

public class ResultModel : IResultModel
{
    protected ResultModel(ErrorResult? error)
    {
        this.ErrorResult = error;
        this.Success = false;
    }

    protected ResultModel()
    {
        this.Success = true;
    }

    public ErrorResult? ErrorResult { get; }

    public bool Success { get; }

    public bool Failure => !this.Success;

    public static IResultModel Ok()
    {
        return new ResultModel();
    }

    public static IResultModel Init => Ok();

    public static IResultModel<T> Ok<T>(T value)
    {
        return new ResultModel<T>(value);
    }

    public static IResultModel Fail(IEnumerable<ErrorResult> errorResults)
    {
        return new ResultModel(errorResults.First()); // TODO: rethink this
    }

    public static IResultModel Fail(ErrorResult? error)
    {
        return new ResultModel(error);
    }

    public static IResultModel<T> Fail<T>(ErrorResult? error)
    {
        return new ResultModel<T>(default!, error);
    }

    public static IResultModel<T> Fail<T>(T value, ErrorResult? error)
    {
        return new ResultModel<T>(value, error);
    }
}

using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.SharedKernel.Results;

public class Result
{
    protected Result(ErrorResult error)
    {
        this.Success = false;
        this.Error = error;
    }

    protected Result()
    {
        this.Success = true;
    }

    public ErrorResult? Error { get; }

    public bool Success { get; }

    public bool Failure => !this.Success;

    public static Result Ok()
    {
        return new();
    }

    public static Result Fail(ErrorResult error)
    {
        return new Result(error);
    }

    public static implicit operator Result(ErrorResult error)
    {
        return new Result(error);
    }

    public static Result<T> Fail<T>(ErrorResult error)
    {
        return new Result<T>(error);
    }

    public Result ToResult()
    {
        throw new NotImplementedException();
    }
}

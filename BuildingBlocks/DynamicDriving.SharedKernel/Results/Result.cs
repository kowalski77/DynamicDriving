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

    public ErrorResult? Error { get; set; }

    public bool Success { get; }

    public bool Failure => !this.Success;

    public static Result Ok() => new();

    public static Result Fail(ErrorResult error)
    {
        return new(error);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new(value);
    }

    public static Result<T> Fail<T>(ErrorResult error)
    {
        return new(error);
    }
}

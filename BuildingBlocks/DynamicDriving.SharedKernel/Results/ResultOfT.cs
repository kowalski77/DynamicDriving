#pragma warning disable CA2225
using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.SharedKernel.Results;

public sealed class Result<T> : Result
{
    private readonly T value;

    public Result(T value)
    {
        this.value = value;
    }

    public Result(ErrorResult error) : base(error)
    {
        this.value = default!;
    }

    public T Value
    {
        get
        {
            if (!this.Success)
            {
                throw new InvalidOperationException("The result object has no value.");
            }

            return this.value;
        }
    }

    public static implicit operator Result<T>(ErrorResult errorResult)
    {
        return new Result<T>(errorResult);
    }

    public static implicit operator Result<T>(T value)
    {
        return new(value);
    }
}

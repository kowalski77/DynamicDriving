using DynamicDriving.SharedKernel.Envelopes;

namespace DynamicDriving.SharedKernel.ResultModels;

public sealed class ResultModel<T> : ResultModel, IResultModel<T>
{
    private readonly T value;

    public ResultModel(
        T value,
        ErrorResult? error) : base(error)
    {
        this.value = value;
    }

    public ResultModel(T value)
    {
        this.value = value;
    }

    public T Value
    {
        get
        {
            if (this.Failure || this.value is null)
            {
                throw new InvalidOperationException("The result object has no value.");
            }

            return this.value;
        }
    }
}

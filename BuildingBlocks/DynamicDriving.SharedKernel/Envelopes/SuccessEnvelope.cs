using System.Diagnostics.CodeAnalysis;

namespace DynamicDriving.SharedKernel.Envelopes;

public class SuccessEnvelope
{
    public static SuccessEnvelope Ok()
    {
        return new SuccessEnvelope();
    }

    public static SuccessEnvelope<T> Create<T>([DisallowNull]T data)
    {
        return new SuccessEnvelope<T>(data);
    }
}

public class SuccessEnvelope<T> : SuccessEnvelope
{
    public SuccessEnvelope([DisallowNull] T data)
    {
        this.Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    public T Data { get; set; }
}

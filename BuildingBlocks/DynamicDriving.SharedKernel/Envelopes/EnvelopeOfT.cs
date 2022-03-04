using System.Diagnostics.CodeAnalysis;

namespace DynamicDriving.SharedKernel.Envelopes;

public class Envelope<T> : Envelope
{
    public Envelope([DisallowNull] T result)
    {
        this.Result = result;
    }

    public T Result { get; set; }
}

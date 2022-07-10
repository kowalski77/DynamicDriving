using System.Runtime.Serialization;

namespace DynamicDriving.Identity.Service.Exceptions;

[Serializable]
public class DeductCreditsException : Exception
{
    public DeductCreditsException()
    {
    }

    public DeductCreditsException(string? message) : base(message)
    {
    }

    public DeductCreditsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DeductCreditsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
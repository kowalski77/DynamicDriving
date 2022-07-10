using System.Runtime.Serialization;

namespace DynamicDriving.Identity.Service.Exceptions;
[Serializable]
internal class AddCreditsException : Exception
{
    public AddCreditsException()
    {
    }

    public AddCreditsException(string? message) : base(message)
    {
    }

    public AddCreditsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AddCreditsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
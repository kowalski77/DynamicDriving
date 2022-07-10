using System.Runtime.Serialization;

namespace DynamicDriving.Identity.Service.Exceptions;

[Serializable]
public class UnknownUserException : Exception
{
    public UnknownUserException(string message) : base(message)
    {
    }

    public UnknownUserException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public UnknownUserException()
    {
    }

    public UnknownUserException(Guid userId) : base($"User with id {userId} does not exist")
    {
        this.UserId = userId;
    }

    protected UnknownUserException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
        throw new NotImplementedException();
    }

    public Guid UserId { get; }
}

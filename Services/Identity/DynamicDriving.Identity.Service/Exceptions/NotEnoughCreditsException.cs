using System.Runtime.Serialization;

namespace DynamicDriving.Identity.Service.Exceptions;

[Serializable]
public class NotEnoughCreditsException : Exception
{
    public NotEnoughCreditsException()
    {
    }

    public NotEnoughCreditsException(string? message) : base(message)
    {
    }

    public NotEnoughCreditsException(Guid userId, int credits) 
        : base($"User {userId} has not enough credits available. Credits available: {credits}")
    {
        this.UserId = userId;
        this.Credits = credits;
    }

    public NotEnoughCreditsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NotEnoughCreditsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public Guid UserId { get; }

    private int Credits { get; }
}
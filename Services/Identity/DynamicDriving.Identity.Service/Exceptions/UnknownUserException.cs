namespace DynamicDriving.Identity.Service.Exceptions;

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

    public UnknownUserException(Guid userId) : this($"User with id {userId} does not exist")
    {
        this.UserId = userId;
    }

    public Guid UserId { get; }
}

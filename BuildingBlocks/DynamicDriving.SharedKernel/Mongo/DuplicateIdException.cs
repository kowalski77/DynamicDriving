namespace DynamicDriving.SharedKernel.Mongo;

public class DuplicateIdException : Exception
{
    public DuplicateIdException(string message) : base(message)
    {
    }

    public DuplicateIdException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DuplicateIdException()
    {
    }
}

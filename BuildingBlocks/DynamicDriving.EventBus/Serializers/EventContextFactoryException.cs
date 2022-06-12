namespace DynamicDriving.EventBus.Serializers;

public class EventContextFactoryException : InvalidOperationException
{
    public EventContextFactoryException(string message) : base(message)
    {
    }

    public EventContextFactoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public EventContextFactoryException()
    {
    }
}

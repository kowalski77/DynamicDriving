namespace DynamicDriving.SharedKernel.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }

    public Guid TransactionId { get; set; }

    public DateTime OccurredOn { get; private set; }

    public string Type { get; private set; } 

    public string Data { get; private set; }

    public EventState State { get; internal set; }

    internal OutboxMessage(
        Guid transactionId,
        DateTime occurredOn, 
        string type, 
        string data)
    {
        this.Id = Guid.NewGuid();
        this.TransactionId = transactionId;
        this.OccurredOn = occurredOn;
        this.Type = type;
        this.Data = data;
        this.State = EventState.NotPublished;
    }
}

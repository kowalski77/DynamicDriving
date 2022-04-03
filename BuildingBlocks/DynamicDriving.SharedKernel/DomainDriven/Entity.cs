using MediatR;

namespace DynamicDriving.SharedKernel.DomainDriven;

public abstract class Entity
{
    private readonly List<INotification> domainEvents = new();

    public IEnumerable<INotification> DomainEvents => this.domainEvents;

    public bool SoftDeleted { get; protected set; }

    protected void AddDomainEvent(INotification eventItem)
    {
        this.domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }
}

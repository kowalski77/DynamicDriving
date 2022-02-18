using MediatR;

namespace DynamicDriving.SharedKernel.Repositories;

public abstract class Entity
{
    private readonly List<INotification> domainEvents = new();

    protected Entity()
    {
    }

    protected Entity(Guid id)
        : this()
    {
        this.Id = id;
    }

    public IEnumerable<INotification> DomainEvents => this.domainEvents;

    public Guid Id { get; protected set; } = Guid.NewGuid();

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

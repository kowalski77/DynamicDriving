#pragma warning disable 8618
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.UsersAggregate;

public sealed class User : Entity, IAggregateRoot
{
    private User() { }

    public User(string name)
    {
        this.Name = name;
    }

    public string Name { get; private set; }
}

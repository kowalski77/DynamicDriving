using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.UsersAggregate;

public class User : Entity, IAggregateRoot
{
    public User(string name)
    {
        this.Name = name;
    }

    public string Name { get; private set; }
}

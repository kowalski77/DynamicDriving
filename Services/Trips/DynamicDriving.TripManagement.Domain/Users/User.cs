using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public Guid Id { get; set; }
}

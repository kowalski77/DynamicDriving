using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.Cars;

public class Car : Entity, IAggregateRoot
{
    public Guid Id { get; set; }
}

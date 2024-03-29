﻿using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.CitiesAggregate;

public class City : Entity, IAggregateRoot
{
    public City(string name)
    {
        this.Name = Guards.ThrowIfNullOrEmpty(name);
        this.Active = true;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public bool Active { get; private set; }
}

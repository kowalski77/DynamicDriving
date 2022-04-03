﻿#pragma warning disable 8618
using DynamicDriving.SharedKernel.DomainDriven;

namespace DynamicDriving.TripManagement.Domain.DriversAggregate;

public sealed class Driver : Entity, IAggregateRoot
{
    public Driver() { }

    public Driver(string name, string description, Car car)
    {
        this.Name = name;
        this.Description = description;
        this.Car = car;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; }

    public string Description { get; private set; }

    public Car Car { get; private set; }
}

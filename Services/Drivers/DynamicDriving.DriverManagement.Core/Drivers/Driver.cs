﻿using DynamicDriving.SharedKernel.Mongo;

namespace DynamicDriving.DriverManagement.Core.Drivers;

public record Driver(Guid Id, string Name, Car Car, bool IsAvailable) : IEntity;

public record Car(Guid Id, string Model, CarType CarType);

public enum CarType
{
    None,
    Sedan,
    Coupe,
    SportsCar,
    StationWagon,
    HatchBack,
    Convertible,
    Suv,
    Minivan,
    Pickup
}

﻿namespace DynamicDriving.Events;

public sealed record TripCreated(Guid Id, Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : IIntegrationEvent;

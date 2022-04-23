using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.TripManagement.Domain.Common;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Events;

public record TripConfirmed(Guid TripId, DateTime PickUp, Coordinates Origin, Coordinates Destination) : IDomainNotification;

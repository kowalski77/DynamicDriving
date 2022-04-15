using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

public sealed record CreateDraftTrip(Guid TripId, Guid UserId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) 
    : ICommand<Result<DraftTripDto>>;

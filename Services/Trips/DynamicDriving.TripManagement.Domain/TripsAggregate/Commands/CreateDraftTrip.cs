using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

public sealed record CreateDraftTrip(Guid UserId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) 
    : ICommand<IResultModel<DraftTripDto>>;

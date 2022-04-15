using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

public record ConfirmTrip(Guid TripId) : ICommand<Result>;


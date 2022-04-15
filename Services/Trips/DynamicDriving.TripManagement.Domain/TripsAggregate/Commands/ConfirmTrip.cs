using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;

public record ConfirmTrip(Guid TripId) : ICommand<IResultModel>;


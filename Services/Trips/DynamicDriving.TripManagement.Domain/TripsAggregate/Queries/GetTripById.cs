using DynamicDriving.SharedKernel.ResultModels;
using MediatR;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;

public record GetTripById(Guid Id) : IRequest<IResultModel<TripByIdDto>>;

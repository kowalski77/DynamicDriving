using DynamicDriving.SharedKernel.Results;
using MediatR;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;

public record GetTripById(Guid Id) : IRequest<Result<TripByIdDto>>;

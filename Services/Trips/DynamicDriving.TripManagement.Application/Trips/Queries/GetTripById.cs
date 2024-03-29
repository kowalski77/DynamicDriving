﻿using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Queries;

public record GetTripById(Guid Id) : IRequest<Result<TripDto>>;

public sealed class GetTripByIdHandler : IRequestHandler<GetTripById, Result<TripDto>>
{
    private readonly ITripReadRepository repository;

    public GetTripByIdHandler(ITripReadRepository repository)
    {
        this.repository = Guards.ThrowIfNull(repository);
    }

    public async Task<Result<TripDto>> Handle(GetTripById request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var resultModel = await Result.Init
            .Do(async () => await this.GetTripById(request.Id, cancellationToken))
            .OnSuccess(trip => trip.AsDto());

        return resultModel;
    }

    private async Task<Result<Trip>> GetTripById(Guid tripId, CancellationToken cancellationToken)
    {
        var maybeTrip = await this.repository.GetByIdAsync(tripId, cancellationToken);

        return maybeTrip.HasValue ?
            maybeTrip.Value : 
            GeneralErrors.NotFound(tripId, nameof(tripId));
    }
}

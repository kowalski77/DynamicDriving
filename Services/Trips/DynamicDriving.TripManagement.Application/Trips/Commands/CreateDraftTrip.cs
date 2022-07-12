using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.Application.Outbox;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed record CreateDraftTrip(Guid UserId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) 
    : ICommand<Result<DraftTripDto>>;

public sealed class CreateDraftTripHandler : ICommandHandler<CreateDraftTrip, Result<DraftTripDto>>
{
    private readonly ITripService tripService;
    private readonly ITripRepository tripRepository;
    private readonly IOutboxService outboxService;

    public CreateDraftTripHandler(ITripService tripService, ITripRepository tripRepository, IOutboxService outboxService)
    {
        this.tripService = Guards.ThrowIfNull(tripService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
        this.outboxService = outboxService;
    }

    public async Task<Result<DraftTripDto>> Handle(CreateDraftTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var originCoordinates = Coordinates.CreateInstance(request.OriginLatitude, request.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(request.DestinationLatitude, request.DestinationLongitude);
        var userId = UserId.CreateInstance(request.UserId);

        var resultModel = await Result.Init
            .Validate(originCoordinates, destinationCoordinates, userId)
            .OnSuccess(async () => await this.CreateTripAsync(userId.Value, request.PickUp, originCoordinates.Value, destinationCoordinates.Value, cancellationToken))
            .OnSuccess(trip => new DraftTripDto(trip.Id));

        return resultModel;
    }

    private async Task<Result<Trip>> CreateTripAsync(UserId userId, DateTime pickUp, Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var result = await this.tripService.CreateDraftTripAsync(userId, pickUp, origin, destination, cancellationToken);
        if (result.Failure)
        {
            return result.Error!;
        }

        var trip = this.tripRepository.Add(result.Value);
        await this.outboxService.AddIntegrationEventAsync(new TripDrafted(trip.Id, trip.CreditsCost ?? 0), cancellationToken);
        
        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return trip;
    }
}

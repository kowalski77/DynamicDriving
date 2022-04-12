using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed class CreateDraftTripHandler : ICommandHandler<CreateDraftTrip, IResultModel<DraftTripDto>>
{
    private readonly ITripService tripService;
    private readonly ITripRepository tripRepository;

    public CreateDraftTripHandler(ITripService tripService, ITripRepository tripRepository)
    {
        this.tripService = Guards.ThrowIfNull(tripService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<IResultModel<DraftTripDto>> Handle(CreateDraftTrip request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var originCoordinates = Coordinates.CreateInstance(request.OriginLatitude, request.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(request.DestinationLatitude, request.DestinationLongitude);
        var userId = UserId.CreateInstance(request.UserId);

        var resultModel = await ResultModel.Init
            .Validate(originCoordinates, destinationCoordinates, userId)
            .OnSuccess(async () => await this.CreateTripAsync(request.TripId, userId.Value, request.PickUp, originCoordinates.Value, destinationCoordinates.Value, cancellationToken))
            .OnSuccess(trip => new DraftTripDto(trip.Id));

        return resultModel;
    }

    private async Task<IResultModel<Trip>> CreateTripAsync(Guid tripId, UserId userId, DateTime pickUp, Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var result = await this.tripService.CreateDraftTripAsync(tripId, userId, pickUp, origin, destination, cancellationToken);
        if (result.Failure)
        {
            return ResultModel.Fail<Trip>(result.Error);
        }

        var trip = this.tripRepository.Add(result.Value);
        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultModel.Ok(trip);
    }
}

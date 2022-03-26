using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.Domain.Common;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Services;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed class CreateDraftTripHandler : ICommandHandler<CreateDraftTrip, IResultModel<DraftTripDto>>
{
    private readonly ITripService tripService;
    private readonly ITripRepository tripRepository;
    private readonly IUserRepository userRepository;

    public CreateDraftTripHandler(ITripService tripService, ITripRepository tripRepository, IUserRepository userRepository)
    {
        this.tripService = Guards.ThrowIfNull(tripService);
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
        this.userRepository = Guards.ThrowIfNull(userRepository);
    }

    public async Task<IResultModel<DraftTripDto>> Handle(CreateDraftTrip request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var originCoordinates = Coordinates.CreateInstance(request.OriginLatitude, request.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(request.DestinationLatitude, request.DestinationLongitude);

        var resultModel = await ResultModel.Init
            .Validate(originCoordinates, destinationCoordinates)
            .OnSuccess(async () => await this.GetUserAsync(request.UserId, cancellationToken))
            .OnSuccess(async user => await this.CreateTripAsync(user, request.PickUp, originCoordinates.Value, destinationCoordinates.Value, cancellationToken))
            .OnSuccess(trip => new DraftTripDto(trip.Id));

        return resultModel;
    }

    private async Task<IResultModel<User>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var maybeUser = await this.userRepository.GetAsync(userId, cancellationToken);

        return !maybeUser.TryGetValue(out var user) ?
            ResultModel.Fail<User>(GeneralErrors.NotFound(userId)) :
            ResultModel.Ok(user);
    }

    private async Task<IResultModel<Trip>> CreateTripAsync(User user, DateTime pickUp, Coordinates origin, Coordinates destination, CancellationToken cancellationToken)
    {
        var result = await this.tripService.CreateDraftTripAsync(user, pickUp, origin, destination, cancellationToken);
        if (result.Failure)
        {
            return ResultModel.Fail<Trip>(result.Error);
        }

        var trip = this.tripRepository.Add(result.Value);
        await this.tripRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultModel.Ok(trip);
    }
}

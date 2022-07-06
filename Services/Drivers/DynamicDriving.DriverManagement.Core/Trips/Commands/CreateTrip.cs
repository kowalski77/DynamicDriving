using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record CreateTrip(Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : ICommand<Result>;

public sealed class CreateTripServiceCommand : IServiceCommand<CreateTrip, Result>
{
    private readonly ITripRepository tripRepository;

    public CreateTripServiceCommand(ITripRepository tripRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task<Result> ExecuteAsync(CreateTrip command, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(command);

        var originCoordinates = Coordinates.CreateInstance(command.OriginLatitude, command.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(command.DestinationLatitude, command.DestinationLongitude);
        var trip = new Trip(command.TripId, command.PickUp, originCoordinates, destinationCoordinates);

        await this.tripRepository.CreateAsync(trip, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}

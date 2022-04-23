using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record CreateTrip(Guid Id, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : ICommand<Guid>;

public sealed class CreateTripHandler : ICommandHandler<CreateTrip, Guid>
{
    private readonly ITripsRepository tripsRepository;

    public CreateTripHandler(ITripsRepository tripsRepository)
    {
        this.tripsRepository = Guards.ThrowIfNull(tripsRepository);
    }

    public async Task<Guid> Handle(CreateTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var originCoordinates = new Coordinates(request.OriginLatitude, request.OriginLongitude);
        var destinationCoordinates = new Coordinates(request.DestinationLatitude, request.DestinationLongitude);
        var trip = new Trip(request.Id, request.PickUp, originCoordinates, destinationCoordinates);

        var tripAdded = await this.tripsRepository.AddAsync(trip, cancellationToken);

        return tripAdded.Id;
    }
}

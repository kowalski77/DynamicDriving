using DynamicDriving.SharedKernel;
using MediatR;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record CreateTrip(Guid Id, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : INotification;

public sealed class CreateTripHandler : INotificationHandler<CreateTrip>
{
    private readonly ITripRepository tripRepository;

    public CreateTripHandler(ITripRepository tripRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task Handle(CreateTrip request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var originCoordinates = Coordinates.CreateInstance(request.OriginLatitude, request.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(request.DestinationLatitude, request.DestinationLongitude);
        var trip = new Trip(request.Id, request.PickUp, originCoordinates, destinationCoordinates);

        await this.tripRepository.CreateAsync(trip, cancellationToken);
    }
}

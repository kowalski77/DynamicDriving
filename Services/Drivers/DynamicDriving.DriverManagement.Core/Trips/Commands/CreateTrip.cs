using DynamicDriving.SharedKernel;
using MediatR;

namespace DynamicDriving.DriverManagement.Core.Trips.Commands;

public sealed record CreateTrip(Guid TripId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude) : INotification;

public sealed class CreateTripHandler : INotificationHandler<CreateTrip>
{
    private readonly ITripRepository tripRepository;

    public CreateTripHandler(ITripRepository tripRepository)
    {
        this.tripRepository = Guards.ThrowIfNull(tripRepository);
    }

    public async Task Handle(CreateTrip notification, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(notification);

        var originCoordinates = Coordinates.CreateInstance(notification.OriginLatitude, notification.OriginLongitude);
        var destinationCoordinates = Coordinates.CreateInstance(notification.DestinationLatitude, notification.DestinationLongitude);
        var trip = new Trip(notification.TripId, notification.PickUp, originCoordinates, destinationCoordinates);

        await this.tripRepository.CreateAsync(trip, cancellationToken).ConfigureAwait(false);
    }
}

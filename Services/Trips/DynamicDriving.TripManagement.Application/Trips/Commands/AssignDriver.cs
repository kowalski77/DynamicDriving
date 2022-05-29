using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed record AssignDriver(Guid TripId, Guid DriverId) : INotification;

public sealed class AssignDriverHandler : INotificationHandler<AssignDriver>
{
    public Task Handle(AssignDriver notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

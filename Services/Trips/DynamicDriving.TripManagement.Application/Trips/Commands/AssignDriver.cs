using MediatR;

namespace DynamicDriving.TripManagement.Application.Trips.Commands;

public sealed record AssignDriver(Guid TripId, Guid DriverId, string DriverName, string Car) : INotification;

public sealed class AssignDriverHandler : INotificationHandler<AssignDriver>
{
    public Task Handle(AssignDriver notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

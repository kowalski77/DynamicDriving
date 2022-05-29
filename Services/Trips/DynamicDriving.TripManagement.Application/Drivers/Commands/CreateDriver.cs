using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using MediatR;

namespace DynamicDriving.TripManagement.Application.Drivers.Commands;

public sealed record CreateDriver(Guid DriverId, string Name, string CarName, string CarDescription) : INotification;

public sealed class CreateDriverHandler : INotificationHandler<CreateDriver>
{
    private readonly IDriverRepository driverRepository;

    public CreateDriverHandler(IDriverRepository driverRepository)
    {
        this.driverRepository = Guards.ThrowIfNull(driverRepository);
    }

    public async Task Handle(CreateDriver notification, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(notification);

        var car = new Car(notification.CarName, notification.CarDescription);
        var driver = new Driver(notification.DriverId, notification.Name, string.Empty, car);

        this.driverRepository.Add(driver);
        await this.driverRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

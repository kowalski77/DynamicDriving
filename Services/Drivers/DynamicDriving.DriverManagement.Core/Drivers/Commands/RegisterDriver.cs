using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Drivers.Commands;

public record RegisterDriver(Guid Id, string Name, CarModel Car, bool IsAvailable) : ICommand<Result<Guid>>;

public record CarModel(string Model, CarType CarType);

public class RegisterDriverHandler : ICommandHandler<RegisterDriver, Result<Guid>>
{
    private readonly IDriverRepository driverRepository;
    private readonly IOutboxService outboxService;

    public RegisterDriverHandler(IDriverRepository driverRepository, IOutboxService outboxService)
    {
        this.driverRepository = Guards.ThrowIfNull(driverRepository);
        this.outboxService = Guards.ThrowIfNull(outboxService);
    }

    public async Task<Result<Guid>> Handle(RegisterDriver request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var car = new Car(Guid.NewGuid(), request.Car.Model, request.Car.CarType);
        var driver = new Driver(request.Id, request.Name, car, request.IsAvailable);

        await this.driverRepository.CreateAsync(driver, cancellationToken).ConfigureAwait(false);

        var driverCreated = new DriverCreated(Guid.NewGuid(), driver.Id, driver.Name, car.Model, car.CarType.ToString());
        await this.outboxService.PublishIntegrationEventAsync(driverCreated, cancellationToken).ConfigureAwait(false);

        return Result.Ok(driver.Id);
    }
}

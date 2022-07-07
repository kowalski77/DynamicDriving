using DynamicDriving.Contracts.Events;
using DynamicDriving.DriverManagement.Core.Outbox;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Drivers.Commands;

public sealed record RegisterDriver(Guid Id, string Name, CarModel Car, bool IsAvailable) : ICommand<Result<Guid>>;

public record CarModel(string Model, CarType CarType);

public sealed class RegisterDriverServiceCommand : IServiceCommand<RegisterDriver, Result<Guid>>
{
    private readonly IDriverRepository driverRepository;
    private readonly IOutboxService outboxService;

    public RegisterDriverServiceCommand(IDriverRepository driverRepository, IOutboxService outboxService)
    {
        this.driverRepository = driverRepository;
        this.outboxService = outboxService;
    }

    public async Task<Result<Guid>> ExecuteAsync(RegisterDriver command, CancellationToken cancellationToken = default)
    {
        Guards.ThrowIfNull(command);

        var car = new Car(Guid.NewGuid(), command.Car.Model, command.Car.CarType);
        var driver = new Driver(command.Id, command.Name, car, command.IsAvailable);

        await this.driverRepository.CreateAsync(driver, cancellationToken).ConfigureAwait(false);

        var driverCreated = new DriverCreated(driver.Id, driver.Name, car.Model, car.CarType.ToString());
        await this.outboxService.PublishIntegrationEventAsync(driverCreated, cancellationToken).ConfigureAwait(false);

        return Result.Ok(driver.Id);
    }
}

using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Mediator;
using DynamicDriving.SharedKernel.Results;

namespace DynamicDriving.DriverManagement.Core.Drivers.Commands;

public record RegisterDriver(Guid Id, string Name, CarModel Car, bool IsAvailable) : ICommand<Result<Guid>>;

public record CarModel(string Model, CarType CarType);

public class RegisterDriverHandler : ICommandHandler<RegisterDriver, Result<Guid>>
{
    private readonly IDriverRepository driverRepository;

    public RegisterDriverHandler(IDriverRepository driverRepository)
    {
        this.driverRepository = Guards.ThrowIfNull(driverRepository);
    }

    public async Task<Result<Guid>> Handle(RegisterDriver request, CancellationToken cancellationToken)
    {
        Guards.ThrowIfNull(request);

        var car = new Car(Guid.NewGuid(), request.Car.Model, request.Car.CarType);
        var driver = new Driver(request.Id, request.Name, car, request.IsAvailable);

        await this.driverRepository.CreateAsync(driver, cancellationToken);

        return Result.Ok(driver.Id);
    }
}

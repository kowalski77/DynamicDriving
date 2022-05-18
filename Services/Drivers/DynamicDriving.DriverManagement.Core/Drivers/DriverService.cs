using DynamicDriving.SharedKernel;

namespace DynamicDriving.DriverManagement.Core.Drivers;

public class DriverService : IDriverService
{
    private readonly IDriverRepository driverRepository;

    public DriverService(IDriverRepository driverRepository)
    {
        this.driverRepository = Guards.ThrowIfNull(driverRepository);
    }

    public async Task<Driver?> GetFirstAvailableDriverAsync(CancellationToken cancellationToken = default)
    {
        // NOTE: this should be more complex than this (more logic), because this is a domain service.
        var allAvailableDrivers = await this.driverRepository.GetAllAsync(x => x.IsAvailable, cancellationToken);
        var driver = allAvailableDrivers.FirstOrDefault();

        return driver;
    }
}

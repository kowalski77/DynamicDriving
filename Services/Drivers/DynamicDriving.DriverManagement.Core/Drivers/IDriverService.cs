namespace DynamicDriving.DriverManagement.Core.Drivers;

public interface IDriverService
{
    Task<Driver?> GetFirstAvailableDriverAsync(CancellationToken cancellationToken = default);
}

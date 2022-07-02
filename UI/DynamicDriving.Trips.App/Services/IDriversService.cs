using DynamicDriving.Models;

namespace DynamicDriving.Trips.App.Services;
public interface IDriversService
{
    Task<IEnumerable<DriverSummary>> GetDriversSummaryAsync();
}
using DynamicDriving.Contracts.Models;
using DynamicDriving.DriverManagement.Core.Drivers;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.GetAll;

public static class GetAllMapper
{
    public static DriversSummaryResponse AsResponse(this IReadOnlyList<DriverSummaryDto> summaryDtos)
    {
        return new DriversSummaryResponse(
            summaryDtos.Select(x => new DriverSummary(
                x.Id, 
                x.Name, 
                x.Car, 
                x.IsAvailable)));
    }
}

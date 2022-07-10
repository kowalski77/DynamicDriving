using DynamicDriving.Contracts.Drivers;
using DynamicDriving.DriverManagement.Core.Drivers;
using DynamicDriving.DriverManagement.Core.Drivers.Queries;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Envelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.GetAll;

[Route("api/v1/[controller]")]
[Authorize(DriverManagementConstants.ReadPolicy)]
public class DriversController : ApplicationController
{
    private readonly IServiceCommand<GetAllDrivers, IReadOnlyList<DriverSummaryDto>> serviceCommand;

    public DriversController(IServiceCommand<GetAllDrivers, IReadOnlyList<DriverSummaryDto>> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    [HttpGet(Name = nameof(GetAllDrivers))]
    [ProducesResponseType(typeof(SuccessEnvelope<DriversSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDrivers()
    {
        var summaries = await this.serviceCommand.ExecuteAsync(new GetAllDrivers()).ConfigureAwait(false);

        return Ok(summaries.AsResponse());
    }
}

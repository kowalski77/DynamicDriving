using DynamicDriving.Contracts.Models;
using DynamicDriving.DriverManagement.Core.Trips;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Assign;

[Route("api/v1/[controller]")]
[Authorize(DriverManagementConstants.WritePolicy)]
public class TripsController : ApplicationController
{
    private readonly IServiceCommand<AssignDriver, Result<AssignDriverDto>> serviceCommand;

    public TripsController(IServiceCommand<AssignDriver, Result<AssignDriverDto>> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<AssignDriverResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignDriver([FromBody] AssignDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        AssignDriver command = request.AsCommand();
        var result = await this.serviceCommand.ExecuteAsync(command).ConfigureAwait(false);

        return FromResult(result, value => new AssignDriverResponse(value.TripId, value.DriverId));
    }
}

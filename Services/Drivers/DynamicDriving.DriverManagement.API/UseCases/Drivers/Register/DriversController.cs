using DynamicDriving.DriverManagement.Core.Drivers.Commands;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Application;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;

[Route("api/v1/[controller]")]
[Authorize(DriverManagementConstants.WritePolicy)]
public class DriversController : ApplicationController
{
    private readonly IServiceCommand<RegisterDriver, Result<Guid>> serviceCommand;

    public DriversController(IServiceCommand<RegisterDriver, Result<Guid>> serviceCommand)
    {
        this.serviceCommand = serviceCommand;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDriver([FromBody] RegisterDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        RegisterDriver command = request.AsCommand();
        var result = await this.serviceCommand.ExecuteAsync(command).ConfigureAwait(false);

        return FromResult(result, value => new RegisterDriverResponse(value));
    }
}

using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;

[Route("api/v1/[controller]")]
public class DriversController : ApplicationController
{
    public DriversController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDriver([FromBody] RegisterDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        var command = request.AsCommand();
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return FromResult(result, value => new RegisterDriverResponse(value));
    }
}

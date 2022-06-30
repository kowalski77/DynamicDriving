using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Assign;

[Route("api/v1/[controller]")]
[Authorize(DriverManagementConstants.WritePolicy)]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<AssignDriverResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignDriver([FromBody] AssignDriverRequest request)
    {
        Guards.ThrowIfNull(request);

        AssignDriver command = request.AsCommand();
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return FromResult(result, value => new AssignDriverResponse(value.TripId, value.DriverId));
    }
}

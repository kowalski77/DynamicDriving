using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Confirm;

[Route("api/v1/[controller]")]
[Authorize(TripManagementConstants.WritePolicy)]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPut("{id:guid}/confirmation")]
    [ProducesResponseType(typeof(SuccessEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmTrip(Guid id)
    {
        Guards.ThrowIfEmpty(id);

        ConfirmTrip command = new(id);
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return FromResult(result);
    }
}

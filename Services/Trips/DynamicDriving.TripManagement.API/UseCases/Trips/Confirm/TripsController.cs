using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.API.Support;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Confirm;

[Route("api/v1/[controller]")]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SuccessEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmTrip(Guid id)
    {
        Guards.ThrowIfEmpty(id);

        var command = new ConfirmTrip(id);
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return this.FromResult(result);
    }
}

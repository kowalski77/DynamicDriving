using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.API.Support;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

[Route("api/v1/[controller]")]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("draft")]
    [ProducesDefaultResponseType(typeof(Envelope))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDraftTrip([FromBody] CreateDraftTripModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var command = model.AsCommand();
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return FromResultModel(result); // TODO: change to this.CreatedResultModel when GetById is available
    }
}

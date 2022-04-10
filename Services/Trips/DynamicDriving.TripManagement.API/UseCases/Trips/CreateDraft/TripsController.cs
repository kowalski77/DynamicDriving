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

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDraftTrip([FromBody] CreateDraftTripRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = request.AsCommand();
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return this.CreatedResultModel(
            result,
            dto => dto.AsResponse(),
            nameof(GetTrip.TripsController.GetTripById),
            () => new { id = result.Value.Id });
    }
}

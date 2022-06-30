using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

[Route("api/v1/[controller]")]
[Authorize]
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
        _ = Guards.ThrowIfNull(request);

        var command = request.AsCommand(this.GetCurrentUserIdBySub());
        var result = await this.Mediator.Send(command).ConfigureAwait(false);

        return this.CreatedResult(
            result,
            dto => dto.AsResponse(),
            nameof(GetById.TripsController.GetTripById),
            () => new { id = result.Value.Id });
    }
}

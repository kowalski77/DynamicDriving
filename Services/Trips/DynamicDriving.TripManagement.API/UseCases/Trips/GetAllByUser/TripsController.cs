using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Application.Trips.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetAllByUser;

[Route("api/v1/[controller]")]
[Authorize(TripManagementConstants.ReadPolicy)]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("me", Name = nameof(GetCurrentUserTrips))]
    [ProducesResponseType(typeof(SuccessEnvelope<TripsByUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUserTrips()
    {
        var userId = this.GetCurrentUserIdBySub();
        var trips = await this.Mediator.Send(new GetTripByUser(userId)).ConfigureAwait(false);

        return Ok(trips.AsResponse(userId));
    }
}

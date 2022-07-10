using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Application.Trips.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetById;

[Route("api/v1/[controller]")]
[Authorize]
public class TripsController : ApplicationController
{
    private readonly IMediator mediator;

    public TripsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id:guid}", Name = nameof(GetTripById))]
    [ProducesResponseType(typeof(SuccessEnvelope<TripByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById(Guid id)
    {
        Guards.ThrowIfEmpty(id);

        var result = await this.mediator.Send(new GetTripById(id)).ConfigureAwait(false);

        return FromResult(result, dto => dto.AsResponse());
    }
}

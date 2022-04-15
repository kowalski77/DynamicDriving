using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.API.Support;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetById;

[Route("api/v1/[controller]")]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:guid}", Name = nameof(GetTripById))]
    [ProducesResponseType(typeof(SuccessEnvelope<TripByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById(Guid id)
    {
        Guards.ThrowIfEmpty(id);

        var resultModel = await this.Mediator.Send(new GetTripById(id)).ConfigureAwait(false);

        return FromResult(resultModel, dto => dto.AsResponse());
    }
}

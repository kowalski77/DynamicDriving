﻿using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

[Route("api/v1/[controller]")]
[Authorize(TripManagementConstants.WritePolicy)]
public class TripsController : ApplicationController
{
    private readonly IMediator mediator;
    
    public TripsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SuccessEnvelope<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorEnvelope), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDraftTrip([FromBody] CreateDraftTripRequest request)
    {
        Guards.ThrowIfNull(request);

        CreateDraftTrip command = request.AsCommand(this.GetCurrentUser());
        var result = await this.mediator.Send(command).ConfigureAwait(false);

        return this.CreatedResult(
            result,
            dto => dto.AsResponse(),
            nameof(GetById.TripsController.GetTripById),
            () => new { id = result.Value.Id });
    }
}

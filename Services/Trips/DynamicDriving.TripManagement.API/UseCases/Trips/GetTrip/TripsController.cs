﻿using DynamicDriving.Models;
using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.API.Support;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.GetTrip;

[Route("api/v1/[controller]")]
public class TripsController : ApplicationController
{
    public TripsController(IMediator mediator) : base(mediator)
    {
    } 

    [HttpGet("{id:guid}")]
    [ProducesDefaultResponseType(typeof(Envelope<TripByIdResponse>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById(Guid id)
    {
        Guards.ThrowIfEmpty(id);

        var resultModel = await this.Mediator.Send(new GetTripById(id)).ConfigureAwait(false);

        return FromResultModel(resultModel, dto => dto.AsResponse());
    }
}
﻿using System.Security.Claims;
using DynamicDriving.Contracts.TripService;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripService.API.StateMachines;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IRequestClient<GetBookingState> bookingClient; // Request/Response in MassTransit

    public BookingsController(IPublishEndpoint publishEndpoint, IRequestClient<GetBookingState> bookingClient)
    {
        this.publishEndpoint = publishEndpoint;
        this.bookingClient = bookingClient;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] SubmitBookingRequest booking)
    {
        Guards.ThrowIfNull(booking);

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var userId = this.User.FindFirstValue("sub");
        var correlationId = Guid.NewGuid(); // TEMP

        var message = new BookingRequested(Guid.Parse(userId), booking.TripId!.Value, booking.Credits, correlationId);
        await this.publishEndpoint.Publish(message).ConfigureAwait(false);

        return this.AcceptedAtAction(
            nameof(GetStatusAsync), 
            new { correlationId }, 
            new { correlationId });
    }

    [HttpGet("status/{correlationId}")]
    public async Task<ActionResult<BookingResponse>> GetStatusAsync(Guid correlationId)
    {
        var bookingStateResponse = await this.bookingClient.GetResponse<BookingState>(new GetBookingState(correlationId)).ConfigureAwait(false);

        var bookingState = bookingStateResponse.Message;
        var response = new BookingResponse(
            bookingState.UserId,
            bookingState.TripId,
            bookingState.Credits,
            bookingState.CurrentState,
            bookingState.ErrorMessage,
            bookingState.Received,
            bookingState.LastUpdated);

        return this.Ok(response);
    }
}
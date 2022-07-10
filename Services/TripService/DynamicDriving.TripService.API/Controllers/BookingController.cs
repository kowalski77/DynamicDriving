using System.Security.Claims;
using DynamicDriving.Contracts.TripService;
using DynamicDriving.SharedKernel;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;

    public BookingController(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SubmitBookingRequest booking)
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

        return this.Accepted();
    }
}
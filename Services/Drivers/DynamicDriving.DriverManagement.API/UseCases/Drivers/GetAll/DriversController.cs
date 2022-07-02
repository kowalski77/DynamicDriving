using DynamicDriving.DriverManagement.Core.Drivers.Queries;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Apis;
using DynamicDriving.SharedKernel.Envelopes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.GetAll;

[Route("api/v1/[controller]")]
[Authorize(DriverManagementConstants.ReadPolicy)]
public class DriversController : ApplicationController
{
    public DriversController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet(Name = nameof(GetAllDrivers))]
    [ProducesResponseType(typeof(SuccessEnvelope<DriversSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDrivers()
    {
        var summaries = await this.Mediator.Send(new GetAllDrivers()).ConfigureAwait(false);

        return Ok(summaries.AsResponse());
    }
}

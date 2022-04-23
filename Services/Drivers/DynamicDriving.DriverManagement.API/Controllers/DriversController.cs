using DynamicDriving.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.DriverManagement.API.Controllers;

public class DriversController : ControllerBase
{
    private readonly IMediator mediator;

    public DriversController(IMediator mediator)
    {
        this.mediator = Guards.ThrowIfNull(mediator);
    }
}

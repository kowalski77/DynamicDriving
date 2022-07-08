using DynamicDriving.Contracts.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MassTransit;
using MediatR;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Assign;

public class DriverAssignedConsumer : IConsumer<DriverAssigned>
{
    private readonly IMediator mediator;

    public DriverAssignedConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DriverAssigned> context)
    {
        Guards.ThrowIfNull(context);

        var command = new AssignDriver(context.Message.TripId, context.Message.DriverId);

        await this.mediator.Send(command).ConfigureAwait(false);
    }
}

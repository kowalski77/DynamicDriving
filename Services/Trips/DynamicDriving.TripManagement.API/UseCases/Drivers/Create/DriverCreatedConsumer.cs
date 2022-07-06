using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Drivers.Commands;
using MassTransit;
using MediatR;

namespace DynamicDriving.TripManagement.API.UseCases.Drivers.Create;

public class DriverCreatedConsumer : IConsumer<DriverCreated>
{
    private readonly IMediator mediator;

    public DriverCreatedConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DriverCreated> context)
    {
        Guards.ThrowIfNull(context);

        CreateDriver command = context.Message.AsCommand();

        await this.mediator.Publish(command).ConfigureAwait(false);
    }
}

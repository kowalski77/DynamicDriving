using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using MassTransit;
using MediatR;

namespace DynamicDriving.DriverManagement.API.UseCases.Trips.Create;

public class TripConfirmedConsumer : IConsumer<TripConfirmed>
{
    private readonly IMediator mediator;

    public TripConfirmedConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<TripConfirmed> context)
    {
        Guards.ThrowIfNull(context);

        CreateTrip command = context.Message.AsCommand();

        await this.mediator.Publish(command).ConfigureAwait(false);
    }
}

using DynamicDriving.Contracts.Trips;
using DynamicDriving.SharedKernel;
using MassTransit;
using MediatR;
using ConfirmTripCommand = DynamicDriving.TripManagement.Application.Trips.Commands.ConfirmTrip;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Confirm;

public class ConfirmTripConsumer : IConsumer<ConfirmTrip>
{
    private readonly IMediator mediator;

    public ConfirmTripConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ConfirmTrip> context)
    {
        Guards.ThrowIfNull(context);

        var command = new ConfirmTripCommand(context.Message.TripId, context.Message.CorrelationId);

        await this.mediator.Send(command).ConfigureAwait(false);
    }
}

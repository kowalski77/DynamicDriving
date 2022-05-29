﻿using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using MediatR;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.Assign;

public class DriverAssignedTranslator : ITranslator<DriverAssigned>
{
    public INotification Translate(DriverAssigned message)
    {
        Guards.ThrowIfNull(message);

        return new AssignDriver(
            message.TripId, 
            message.Driver.Id, 
            message.Driver.Name, 
            message.Driver.Car);
    }
}

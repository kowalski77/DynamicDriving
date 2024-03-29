﻿using System.Runtime.Serialization;

namespace DynamicDriving.TripManagement.Domain.TripsAggregate.Exceptions;

[Serializable]
public class TripConfirmationException : Exception
{
    public TripConfirmationException()
    {
    }

    public TripConfirmationException(string? message) : base(message)
    {
    }

    public TripConfirmationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TripConfirmationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
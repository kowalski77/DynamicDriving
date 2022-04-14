namespace DynamicDriving.TripManagement.Domain.TripsAggregate;

public enum TripStatus
{
    Draft,
    Ordered,
    ToOrigin,
    ToDestination,
    Canceled,
    Finished
}

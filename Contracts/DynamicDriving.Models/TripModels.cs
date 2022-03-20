[assembly: CLSCompliant(false)]

namespace DynamicDriving.Models;

public sealed record CreateDraftTripModel(Guid UserId, DateTime PickUp, decimal OriginLatitude, decimal OriginLongitude, decimal DestinationLatitude, decimal DestinationLongitude);

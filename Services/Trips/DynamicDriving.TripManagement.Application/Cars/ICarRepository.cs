using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;

namespace DynamicDriving.TripManagement.Application.Cars;

public interface ICarRepository : IRepository<Driver>
{
}

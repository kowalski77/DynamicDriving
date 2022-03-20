using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.CarsAggregate;

namespace DynamicDriving.TripManagement.Application.Cars;

public interface ICarRepository : IRepository<Car>
{
}

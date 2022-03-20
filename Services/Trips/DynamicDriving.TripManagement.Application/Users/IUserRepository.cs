using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.UsersAggregate;

namespace DynamicDriving.TripManagement.Application.Users;

public interface IUserRepository : IRepository<User>
{
}

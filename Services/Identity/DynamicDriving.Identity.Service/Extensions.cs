using DynamicDriving.Identity.Service.Entities;

namespace DynamicDriving.Identity.Service;

public static class Extensions
{
    public static UserDto AsDto(this ApplicationUser user)
    {
        return new UserDto(
            user.Id,
             user.UserName,
             user.Email,
             user.CreatedOn);
    }
}

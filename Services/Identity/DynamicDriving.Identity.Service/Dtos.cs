using System.ComponentModel.DataAnnotations;

namespace DynamicDriving.Identity.Service;

public record UserDto(Guid Id, string UserName, string Email, DateTime CreatedAt);

public record UpdateUserDto([Required] [EmailAddress] string Email);
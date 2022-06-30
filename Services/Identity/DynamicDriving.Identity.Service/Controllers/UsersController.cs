using DynamicDriving.Identity.Service.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace DynamicDriving.Identity.Service.Controllers;

[ApiController]
[Route("users")]
[Authorize(Policy = LocalApi.PolicyName, Roles = Roles.Admin)]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> Get()
    {
        var users = this.userManager.Users.ToList().Select(x => x.AsDto());

        return this.Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(Guid id)
    {
        var user = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

        return user is null ?
            this.NotFound() :
            this.Ok(user.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, UpdateUserDto user)
    {
        var userToUpdate = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (userToUpdate is null)
        {
            return this.NotFound();
        }

        userToUpdate.Email = user.Email;
        userToUpdate.UserName = user.Email;
        var result = await this.userManager.UpdateAsync(userToUpdate);

        return result.Succeeded ?
            this.NoContent() :
            this.BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userToDelete = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (userToDelete is null)
        {
            return this.NotFound();
        }

        var result = await this.userManager.DeleteAsync(userToDelete);

        return result.Succeeded ?
            this.NoContent() :
            this.BadRequest(result.Errors);
    }
}

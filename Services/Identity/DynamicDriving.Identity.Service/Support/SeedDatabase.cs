using DynamicDriving.Identity.Service.Entities;
using DynamicDriving.Identity.Service.Settings;
using DynamicDriving.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace DynamicDriving.Identity.Service.Support;

public static class SeedDatabase
{
    public static async Task SeedUserRoles(this WebApplication host)
    {
        _ = Guards.ThrowIfNull(host);

        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        
        var identitySettings = host.Configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>(); 

        await CreateRoleAsync(roleManager, Roles.Admin).ConfigureAwait(false);
        await CreateRoleAsync(roleManager, Roles.User).ConfigureAwait(false);

        await CreateAdminUserAsync(userManager, identitySettings).ConfigureAwait(false);
    }

    private static async Task CreateRoleAsync(RoleManager<ApplicationRole> roleManager, string role)
    {
        var roleExists = await roleManager.RoleExistsAsync(role).ConfigureAwait(false);
        if (roleExists)
        {
            return;
        }

        _ = await roleManager.CreateAsync(new ApplicationRole
        {
            Name = role,
            NormalizedName = role.ToUpperInvariant(),
        }).ConfigureAwait(false);
    }

    private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager, IdentitySettings settings)
    {
        var adminUser = await userManager.FindByEmailAsync(settings.AdminUserEmail).ConfigureAwait(false);
        if (adminUser is not null)
        {
            return;
        }

        adminUser = new ApplicationUser
        {
            UserName = settings.AdminUserEmail,
            Email = settings.AdminUserEmail
        };

        _ = await userManager.CreateAsync(adminUser, settings.AdminUserPassword).ConfigureAwait(false);
        _ = await userManager.AddToRoleAsync(adminUser, Roles.Admin).ConfigureAwait(false);
    }
}

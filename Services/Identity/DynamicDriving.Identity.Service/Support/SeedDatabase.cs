using DynamicDriving.Identity.Service.Entities;
using DynamicDriving.Identity.Service.Settings;
using DynamicDriving.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace DynamicDriving.Identity.Service.Support;

public static class SeedDatabase
{
    public static async Task SeedUsersAndRoles(this WebApplication host)
    {
        Guards.ThrowIfNull(host);

        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        
        var identitySettings = host.Configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>(); 

        await CreateRoleAsync(roleManager, Roles.Admin).ConfigureAwait(false);
        await CreateRoleAsync(roleManager, Roles.User).ConfigureAwait(false);

        await CreateAdminUserAsync(userManager, identitySettings).ConfigureAwait(false);
        await CreateTestUserAsync(userManager).ConfigureAwait(false);
    }

    private static async Task CreateRoleAsync(RoleManager<ApplicationRole> roleManager, string role)
    {
        var roleExists = await roleManager.RoleExistsAsync(role).ConfigureAwait(false);
        if (roleExists)
        {
            return;
        }

        await roleManager.CreateAsync(new ApplicationRole
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
            UserName = "Admin",
            Email = settings.AdminUserEmail,
            Credits = 999_999,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, settings.AdminUserPassword).ConfigureAwait(false);
        await userManager.AddToRoleAsync(adminUser, Roles.Admin).ConfigureAwait(false);
    }

    private static async Task CreateTestUserAsync(UserManager<ApplicationUser> userManager)
    {
        const string testUserEmail = "test@dynamic-driving.com";
        
        var testUser = await userManager.FindByEmailAsync(testUserEmail).ConfigureAwait(false);
        if(testUser is not null)
        {
            return;
        }

        testUser = new ApplicationUser
        {
            UserName = "tester01",
            Email = testUserEmail,
            Credits = 100,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(testUser, "Martin,22").ConfigureAwait(false);
        await userManager.AddToRoleAsync(testUser, Roles.User).ConfigureAwait(false);
    }
}

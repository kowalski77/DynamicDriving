namespace DynamicDriving.DriverManagement.API.Support;

public static class StartupExtensions
{
    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(DriverManagementConstants.ReadPolicy, policy =>
            {
                _ = policy.RequireRole(DriverManagementConstants.AdminRole);
                _ = policy.RequireClaim("scope", "drivermanagement.readaccess", "drivermanagement.fullaccess");
            });
            options.AddPolicy(DriverManagementConstants.WritePolicy, policy =>
            {
                _ = policy.RequireRole(DriverManagementConstants.AdminRole);
                _ = policy.RequireClaim("scope", "drivermanagement.writeaccess", "drivermanagement.fullaccess");
            });
        });
    }
}

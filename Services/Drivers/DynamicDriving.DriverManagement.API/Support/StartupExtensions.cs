namespace DynamicDriving.DriverManagement.API.Support;

public static class StartupExtensions
{
    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(DriverManagementConstants.ReadPolicy, policy =>
            {
                policy.RequireRole(DriverManagementConstants.AdminRole);
                policy.RequireClaim("scope", "drivermanagement.readaccess", "drivermanagement.fullaccess");
            });
            options.AddPolicy(DriverManagementConstants.WritePolicy, policy =>
            {
                policy.RequireRole(DriverManagementConstants.AdminRole);
                policy.RequireClaim("scope", "drivermanagement.writeaccess", "drivermanagement.fullaccess");
            });
        });
    }
}

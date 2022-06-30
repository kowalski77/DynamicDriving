namespace DynamicDriving.TripManagement.API.Support;

public static class StartupExtensions
{
    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(TripManagementConstants.ReadPolicy, policy =>
            {
                _ = policy.RequireClaim("scope", "tripmanagement.readaccess", "tripmanagement.fullaccess");
            });
            options.AddPolicy(TripManagementConstants.WritePolicy, policy =>
            {
                _ = policy.RequireClaim("scope", "tripmanagement.writeaccess", "tripmanagement.fullaccess");
            });
        });
    }
}

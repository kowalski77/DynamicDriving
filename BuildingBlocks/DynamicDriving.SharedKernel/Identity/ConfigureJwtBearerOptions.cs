using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DynamicDriving.SharedKernel.Identity;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IConfiguration configuration;
    
    public ConfigureJwtBearerOptions(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        if(name == JwtBearerDefaults.AuthenticationScheme)
        {
            var identitySettings = this.configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

            options.Authority = identitySettings.Authority;
            options.Audience = identitySettings.Audience;
            // standards
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        this.Configure(Options.DefaultName, options);
    }
}

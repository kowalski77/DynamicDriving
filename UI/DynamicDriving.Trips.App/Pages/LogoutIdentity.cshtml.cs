using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicDriving.Trips.App.Pages;

public class LogoutIdentityModel : PageModel
{
    public async Task OnPostAsync()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
}

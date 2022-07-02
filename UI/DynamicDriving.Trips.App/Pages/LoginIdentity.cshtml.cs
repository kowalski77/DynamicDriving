using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicDriving.Trips.App.Pages;

public class LoginIdentityModel : PageModel
{
    public async Task OnGetAsync(string redirectUri)
    {
        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            redirectUri = this.Url.Content("~/");
        }

        if (this.HttpContext.User.Identity is not null && this.HttpContext.User.Identity.IsAuthenticated)
        {
            this.Response.Redirect(redirectUri);
        }

        await this.HttpContext.ChallengeAsync(
           OpenIdConnectDefaults.AuthenticationScheme,
           new AuthenticationProperties { RedirectUri = redirectUri });
    }
}

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace DynamicDriving.Trips.App.Support;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly TokenProvider tokenProvider;

    public CustomAuthStateProvider(TokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthenticationState authenticationState;
        if (this.tokenProvider.ExpiresAt < DateTime.UtcNow)
        {
            authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            this.NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

            return Task.FromResult(authenticationState);
        }

        authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(this.tokenProvider.AccessToken), "jwtAuthType", ClaimTypes.Name, "roles")));
        this.NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

        return Task.FromResult(authenticationState);
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var result = keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value!.ToString()));

        return result;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}

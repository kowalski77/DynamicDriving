using IdentityModel.Client;

namespace DynamicDriving.Trips.App.Support;

public class TokenManager
{
    private readonly TokenProvider tokenProvider;
    private readonly IHttpClientFactory httpClientFactory;

    public TokenManager(TokenProvider tokenProvider, IHttpClientFactory httpClientFactory)
    {
        this.tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<string> RetrieveAccessTokenAsync()
    {
        // should we refresh
        if (this.tokenProvider.ExpiresAt.AddSeconds(-60).ToUniversalTime() > DateTime.UtcNow)
        {
            // no need to refresh, return the access token
            return this.tokenProvider.AccessToken;
        }

        // refresh
        var idpClient = this.httpClientFactory.CreateClient();
        var discoveryReponse = await idpClient.GetDiscoveryDocumentAsync("https://localhost:7070");

        var refreshResponse = await idpClient.RequestRefreshTokenAsync(
           new RefreshTokenRequest
           {
               Address = discoveryReponse.TokenEndpoint,
               ClientId = "tripsui",
               RefreshToken = this.tokenProvider.RefreshToken
           });

        this.tokenProvider.AccessToken = refreshResponse.AccessToken;
        this.tokenProvider.RefreshToken = refreshResponse.RefreshToken;
        this.tokenProvider.ExpiresAt = DateTime.UtcNow.AddSeconds(refreshResponse.ExpiresIn);

        return this.tokenProvider.AccessToken;
    }
}

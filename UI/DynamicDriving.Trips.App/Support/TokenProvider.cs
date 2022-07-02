namespace DynamicDriving.Trips.App.Support;

public class TokenProvider
{
    public string XsrfToken { get; set; } = default!;
    
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public DateTimeOffset ExpiresAt { get; set; }

}

public class InitialApplicationState
{
    public string XsrfToken { get; set; } = default!;

    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public DateTimeOffset ExpiresAt { get; set; }
}
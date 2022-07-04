using DynamicDriving.Trips.App.Data;
using DynamicDriving.Trips.App.Services;
using DynamicDriving.Trips.App.Support;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddHttpClient<IDriversService, DriversService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7292/");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
      options =>
      {
          options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          options.Authority = "https://localhost:7070";
          options.ClientId = "tripsui";
          options.ResponseType = "code";
          options.Scope.Add("openid");
          options.Scope.Add("profile");
          options.Scope.Add("email");
          options.Scope.Add("drivermanagement.fullaccess");
          options.Scope.Add("offline_access");
          options.SaveTokens = true;
          options.GetClaimsFromUserInfoEndpoint = true;
          options.TokenValidationParameters.NameClaimType = "given_name";
      });

builder.Services.AddScoped<TokenManager>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

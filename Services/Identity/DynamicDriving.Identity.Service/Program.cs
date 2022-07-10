using DynamicDriving.Identity.Service.Consumers;
using DynamicDriving.Identity.Service.Entities;
using DynamicDriving.Identity.Service.Exceptions;
using DynamicDriving.Identity.Service.Settings;
using DynamicDriving.Identity.Service.Support;
using DynamicDriving.MassTransit;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
    (
        connectionString: "mongodb://localhost:27017", // TODO: refactor
        databaseName: "IdentityDb"
    );

var identityServerSettings = builder.Configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>();

builder.Services.AddIdentityServer(options =>
{ // more logs
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;
})
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryApiScopes(identityServerSettings.ApiScopes)
    .AddInMemoryApiResources(identityServerSettings.ApiResources)
    .AddInMemoryClients(identityServerSettings.Clients)
    .AddInMemoryIdentityResources(identityServerSettings.IdentityResources)
    .AddDeveloperSigningCredential(); // not for production

builder.Services.AddLocalApiAuthentication();
builder.Services.AddMassTransitWithRabbitMq(typeof(DeductCreditsConsumer).Assembly, configure =>
{
    configure.ConfigureRetries = cfg => 
    { 
        cfg.Interval(3, TimeSpan.FromSeconds(5));
        cfg.Ignore(typeof(DeductCreditsException), typeof(AddCreditsException), typeof(UnknownUserException));
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins(app.Configuration["AllowedOrigin"])
    .AllowAnyHeader()
    .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    await app.SeedUsersAndRoles().ConfigureAwait(false); // NOTE: is not a good idea in production
}

await app.RunAsync().ConfigureAwait(false);

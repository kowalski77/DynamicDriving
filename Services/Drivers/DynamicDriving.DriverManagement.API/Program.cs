using DynamicDriving.AzureServiceBus;
using DynamicDriving.DriverManagement.API;
using DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;
using DynamicDriving.DriverManagement.API.UseCases.Trips.Create;
using DynamicDriving.DriverManagement.Core;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.DriverManagement.Infrastructure;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Identity;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterDriverValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(CreateTrip).Assembly);
builder.Services.AddTranslator<TripConfirmed, TripConfirmedTranslator>();

var storageConnectionString = builder.Configuration["StorageConnectionString"];
builder.Services.AddAzureServiceBusReceiver(configure =>
{
    configure.StorageConnectionString = storageConnectionString;
    configure.RegisterIntegrationEvent<TripConfirmed>();
});

builder.Services.AddAzureServiceBusPublisher(configure =>
{
    configure.StorageConnectionString = storageConnectionString;
});

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Read, policy =>
    {
        policy.RequireRole(DriverManagementConstants.AdminRole);
        policy.RequireClaim("scope", "drivermanagement.readaccess", "drivermanagement.fullaccess");
    });
    options.AddPolicy(Policies.Write, policy =>
    {
        policy.RequireRole(DriverManagementConstants.AdminRole);
        policy.RequireClaim("scope", "drivermanagement.writeaccess", "drivermanagement.fullaccess");
    });
});
builder.Services.AddJwtBearerAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseMiddleware<ExceptionHandler>();
}

app.UseCors(cfg =>
{
    cfg.WithOrigins(builder.Configuration["AllowedOrigin"])
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }

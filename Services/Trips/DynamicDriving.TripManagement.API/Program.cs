using DynamicDriving.AzureServiceBus;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Identity;
using DynamicDriving.TripManagement.API.UseCases.Drivers.Create;
using DynamicDriving.TripManagement.API.UseCases.Trips.Assign;
using DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;
using DynamicDriving.TripManagement.Application;
using DynamicDriving.TripManagement.Domain;
using DynamicDriving.TripManagement.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateDraftTripValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTranslator<DriverCreated, DriverCreatedTranslator>();
builder.Services.AddTranslator<DriverAssigned, DriverAssignedTranslator>();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddHostedService<OutboxHostedService>();

builder.Services.AddJwtBearerAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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

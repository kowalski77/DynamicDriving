﻿using DynamicDriving.DriverManagement.API.Support;
using DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;
using DynamicDriving.DriverManagement.API.UseCases.Trips.Create;
using DynamicDriving.DriverManagement.Core;
using DynamicDriving.DriverManagement.Core.Infrastructure;
using DynamicDriving.MassTransit;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Identity;
using FluentValidation.AspNetCore;
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

builder.Services.AddMassTransitWithRabbitMq(typeof(TripCreatedConsumer).Assembly);
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCustomAuthorization();
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

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces

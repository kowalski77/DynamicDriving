using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.TripManagement.API.Support;
using DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;
using DynamicDriving.TripManagement.Application;
using DynamicDriving.TripManagement.Domain;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateDraftTripValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MigrateDatabase<>()
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

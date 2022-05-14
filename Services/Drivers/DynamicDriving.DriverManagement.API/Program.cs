using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.DriverManagement.API.Translators;
using DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.DriverManagement.Infrastructure;
using DynamicDriving.Events;
using DynamicDriving.SharedKernel.Envelopes;
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
builder.Services.AddTranslator<TripConfirmed, ExamCreatedTranslator>(); //TODO: Register all by assembly
builder.Services.AddAzureServiceBusReceiver(cfg =>
{
    cfg.StorageConnectionString = builder.Configuration["StorageConnectionString"];
    cfg.MessageProcessors = new[]
    {
        new MessageProcessor("tripconfirmed", typeof(TripConfirmed)) // TODO: rethink
    };
});
builder.Services.AddInfrastructure(builder.Configuration);

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

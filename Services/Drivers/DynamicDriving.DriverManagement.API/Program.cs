using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.DriverManagement.API.Translators;
using DynamicDriving.DriverManagement.Core.Trips.Commands;
using DynamicDriving.DriverManagement.Infrastructure;
using DynamicDriving.Events;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

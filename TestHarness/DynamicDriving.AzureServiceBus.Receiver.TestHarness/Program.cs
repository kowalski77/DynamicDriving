using DynamicDriving.AzureServiceBus.Receiver;
using DynamicDriving.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddAzureServiceBusReceiver(cfg =>
            {
                cfg.StorageConnectionString = builder.Configuration["StorageConnectionString"];
                cfg.MessageProcessors = new[]
                {
                    new MessageProcessor("ping", typeof(Ping))
                };
            });

var app = builder.Build();

app.Run();

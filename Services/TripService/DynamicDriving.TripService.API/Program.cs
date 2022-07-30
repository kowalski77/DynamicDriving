using System.Text.Json.Serialization;
using DynamicDriving.Contracts.Identity;
using DynamicDriving.Contracts.Trips;
using DynamicDriving.MassTransit;
using DynamicDriving.SharedKernel.Identity;
using DynamicDriving.SharedKernel.Logging;
using DynamicDriving.SharedKernel.Mongo;
using DynamicDriving.TripService.API.Consumers;
using DynamicDriving.TripService.API.Entities;
using DynamicDriving.TripService.API.Exceptions;
using DynamicDriving.TripService.API.Settings;
using DynamicDriving.TripService.API.StateMachines;
using MassTransit;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongo().AddMongoRepository<Trip>();
builder.Services.AddJwtBearerAuthentication();
AddMassTransit(builder.Services, builder.Configuration);

builder.Services.AddSeqLogging(builder.Configuration);

builder.Services.AddOpenTelemetryTracing(tracerBuilder =>
{
    tracerBuilder
        .AddSource("TripService")
        .AddSource("MassTransit")
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "TripService"))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddJaegerExporter(options => 
        {
            var jaegerSettings = builder.Configuration.GetSection(nameof(JaegerSettings)).Get<JaegerSettings>();
            options.AgentHost = jaegerSettings.Host;
            options.AgentPort = jaegerSettings.Port;
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddMassTransit(IServiceCollection services, IConfiguration configuration)
{
    services.AddMassTransit(configure =>
    {
        configure.UsingDynamicDrivingRabbitMq(retryConfig =>
        {
            retryConfig.Interval(3, TimeSpan.FromSeconds(5));
            retryConfig.Ignore(typeof(TripNotFoundException));
        });
        configure.AddConsumers(typeof(TripDraftedConsumer).Assembly);
        configure.AddSagaStateMachine<BookingStateMachine, BookingState>(sagaConfigurator =>
        {
            sagaConfigurator.UseInMemoryOutbox(); // No message will be sent from the Saga pipelines until the transition to the state is stored in the database.
                                                  // Ex.  Send(context => new ConfirmTrip(context.Saga.TripId, context.Saga.CorrelationId))
        })
        .MongoDbRepository(r =>
        {
            var mongoOptions = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
            var massTransitSettings = configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>();

            r.Connection = mongoOptions.Client;
            r.DatabaseName = massTransitSettings.ServiceName;
        });
    });

    // Map Commands in MassTransit
    var queueSettings = configuration.GetSection(nameof(QueueSettings)).Get<QueueSettings>();
    EndpointConvention.Map<ConfirmTrip>(new Uri(queueSettings.ConfirmTripQueueAddress!));
    EndpointConvention.Map<DeductCredits>(new Uri(queueSettings.DeductCreditsQueueAddress!));
    EndpointConvention.Map<InvalidateTrip>(new Uri(queueSettings.InvalidateTripQueueAddress!));
}

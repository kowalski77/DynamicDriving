using DynamicDriving.MassTransit;
using DynamicDriving.SharedKernel.Identity;
using DynamicDriving.SharedKernel.Mongo;
using DynamicDriving.TripService.API.StateMachines;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMongo();
builder.Services.AddJwtBearerAuthentication();
AddMassTransit(builder.Services, builder.Configuration);

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
        configure.UsingDynamicDrivingRabbitMq();
        configure.AddSagaStateMachine<BookingStateMachine, BookingState>()
        .MongoDbRepository(r =>
        {
            var mongoOptions = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
            var massTransitSettings = configuration.GetSection(nameof(MassTransitSettings)).Get<MassTransitSettings>();

            r.Connection = mongoOptions.Client;
            r.DatabaseName = massTransitSettings.ServiceName;
        });
    });
}

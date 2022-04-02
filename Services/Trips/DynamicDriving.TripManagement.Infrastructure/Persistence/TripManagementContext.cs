using DynamicDriving.SharedKernel;
using DynamicDriving.SharedKernel.DomainDriven;
using DynamicDriving.TripManagement.Domain.DriversAggregate;
using DynamicDriving.TripManagement.Domain.LocationsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.UsersAggregate;
using DynamicDriving.TripManagement.Infrastructure.Persistence.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.TripManagement.Infrastructure.Persistence;

public sealed class TripManagementContext : TransactionContext
{
    public TripManagementContext(DbContextOptions options, IMediator mediator) 
        : base(options, mediator)
    {
    }

    public DbSet<Trip> Trips { get; set; } = default!;

    public DbSet<Location> Locations { get; set; } = default!;

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Driver> Drivers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guards.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocationEntityTypeConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

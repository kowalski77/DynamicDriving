﻿using DynamicDriving.SharedKernel.DomainDriven;
using Microsoft.EntityFrameworkCore;

namespace DynamicDriving.SharedKernel.Outbox.Sql;

public class OutboxContext : DbContext, IUnitOfWork
{
    public OutboxContext(DbContextOptions<OutboxContext> options) 
        : base(options)
    {
    }

    public DbSet<OutboxMessage> OutboxMessages { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
    }
        
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
            
        return result;
    }
}

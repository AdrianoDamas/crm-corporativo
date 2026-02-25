namespace CRM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CRM.Domain.Entities;
using CRM.Infrastructure.EntityConfigurations;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

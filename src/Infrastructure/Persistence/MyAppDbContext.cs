using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class MyAppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
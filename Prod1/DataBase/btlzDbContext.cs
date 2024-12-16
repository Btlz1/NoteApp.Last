using System.Reflection;
using btlz.Models;
using Microsoft.EntityFrameworkCore;

namespace btlz.Database;

public class btlzDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public btlzDbContext(
        DbContextOptions<btlzDbContext> options) 
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
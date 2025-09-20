using Microsoft.EntityFrameworkCore;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(u => u.Id);
            b.Property(u => u.Name).HasMaxLength(200).IsRequired();
            b.Property(u => u.Email).HasMaxLength(200).IsRequired();
            b.Property(u => u.Balance).HasColumnType("decimal(18,2)");
        });
    }
}

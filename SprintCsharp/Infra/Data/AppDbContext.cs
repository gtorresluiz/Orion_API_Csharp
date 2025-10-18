using Microsoft.EntityFrameworkCore;
using SprintCsharp.Domain.Entities;

namespace SprintCsharp.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("USERS");
                b.HasKey(u => u.Id);

                b.Property(u => u.Id).HasColumnName("ID");
                b.Property(u => u.Name).HasColumnName("NAME").HasMaxLength(100).IsRequired();
                b.Property(u => u.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
                b.Property(u => u.Balance).HasColumnName("BALANCE").HasColumnType("NUMBER(18,2)");
                b.Property(u => u.PreferredInvestment).HasColumnName("PREFERREDINVESTMENT").HasConversion<int>();
                b.Property(u => u.Level).HasColumnName("PROFESSIONLEVEL").HasConversion<int>();
                b.Property(u => u.UpdatedAt).HasColumnName("UPDATEDAT").HasColumnType("TIMESTAMP").IsRequired(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
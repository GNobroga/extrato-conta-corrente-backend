using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{

    public DbSet<Lancamento> Lancamentos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lancamento>()
            .Property(l => l.Data)
            .HasConversion(v => DateTime.SpecifyKind(v, DateTimeKind.Utc), (v) => v);
    }
}
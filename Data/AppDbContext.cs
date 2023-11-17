using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{

    public DbSet<Lancamento> Lancamentos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
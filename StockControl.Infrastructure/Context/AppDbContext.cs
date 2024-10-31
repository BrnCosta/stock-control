using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;

namespace StockControl.Infrastructure.Context
{
  public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
  {
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockHolder> StockHolders { get; set; }
    public DbSet<StockOperation> StockOperations { get; set; }
    public DbSet<Dividend> Dividends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Stock>().HasKey(e => e.Symbol);

      modelBuilder.Entity<StockHolder>()
          .Property(e => e.Id)
          .ValueGeneratedOnAdd();

      modelBuilder.Entity<StockOperation>()
          .Property(e => e.Id)
          .ValueGeneratedOnAdd();

      modelBuilder.Entity<Dividend>()
        .Property(e => e.Id)
        .ValueGeneratedOnAdd();
    }
  }
}

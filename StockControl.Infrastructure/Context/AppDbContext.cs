using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;

namespace StockControl.Infrastructure.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Dividend> Dividends { get; set; }
        public DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(a =>
            {
                a.HasKey(a => a.Id);

                a.HasIndex(a => a.Ticker)
                    .IsUnique();

                a.Property(a => a.Currency)
                    .HasConversion<string>();

                a.Property(a => a.Type)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Position>(p =>
            {
                p.HasKey(p => p.AssetId);

                p.HasOne(p => p.Asset)
                    .WithOne(a => a.Position)
                    .HasForeignKey<Position>(p => p.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Trade>(t =>
            {
                t.HasMany(t => t.Transactions)
                    .WithOne(tr => tr.Trade)
                    .HasForeignKey(tr => tr.TradeId)
                    .OnDelete(DeleteBehavior.Cascade);

                t.HasIndex(t => t.Date);

                t.Property(t => t.Currency)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Transaction>(tr =>
            {
                tr.HasOne(t => t.Asset)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AssetId)
                    .OnDelete(DeleteBehavior.Restrict);

                tr.Property(tr => tr.OperatingType)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Dividend>(d =>
            {
                d.HasKey(d => d.Id);

                d.HasIndex(d => d.AssetId);
            });
        }
    }
}

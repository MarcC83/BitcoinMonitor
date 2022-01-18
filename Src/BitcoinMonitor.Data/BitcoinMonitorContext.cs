using BitcoinMonitor.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMonitor.Data
{
    public class BitcoinMonitorContext : DbContext
    {
        public BitcoinMonitorContext(DbContextOptions<BitcoinMonitorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
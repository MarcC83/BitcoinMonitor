using BitcoinMonitor.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMonitor.Data
{
    public class BitcoinMonitorContext : DbContext
    {
        //here warning is supprocessed since EF core i
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BitcoinMonitorContext(DbContextOptions<BitcoinMonitorContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
using BitcoinMonitor.Domain.Models;

namespace BitcoinMonitor.Data
{
    public class ExchangeRateService
    {
        private readonly BitcoinMonitorContext _context;

        public ExchangeRateService(BitcoinMonitorContext context)
        {
            _context = context;
        }
        public Task<ExchangeRate[]> GetExchangeRatesAsync()
        {
            return Task.Factory.StartNew(() => _context.ExchangeRates.ToArray());             
        }
    }
}

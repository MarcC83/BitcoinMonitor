using BitcoinMonitor.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMonitor.Data
{
    public class ExchangeRateService
    {
        private readonly BitcoinMonitorContext _context;

        public ExchangeRateService(BitcoinMonitorContext context)
        {
            _context = context;
        }
        async public Task<ExchangeRate[]> GetExchangeRatesAsync()
        {
            return await _context.ExchangeRates.ToArrayAsync();             
        }

        async public Task<ExchangeRate[]> GetExchangeRatesAsync(DateTime timeFrom)
        {
            return await _context.ExchangeRates
                                    .Where(er => er.Time > timeFrom.ToUniversalTime())
                                    .ToArrayAsync();
        }
    }
}

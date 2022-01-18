using BitcoinMonitor.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinMonitor.Domain.Interfaces.CurrenciesExchange
{
    public interface IExchangeRateProvider
    {
        Task<ExchangeRate> GetCurrentExchangeRate(string baseCurrency, string targetCurrency);
    }
}

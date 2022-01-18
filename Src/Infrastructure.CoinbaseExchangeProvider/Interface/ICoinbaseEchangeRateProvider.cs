using Infrastructure.CoinbaseExchangeProvider.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CoinbaseExchangeProvider.Interface
{
    public interface ICoinbaseEchangeRateProvider
    {
        [Get("/v2/prices/{baseCurrency}-{targetCurrency}/buy")]
        Task<CoinbaseExchangeResponse> GetCurrentExchangeRate(string baseCurrency, string targetCurrency);
    }
}

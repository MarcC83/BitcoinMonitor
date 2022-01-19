using AutoMapper;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models;
using Infrastructure.CoinbaseExchangeProvider.Interface;
using Microsoft.Extensions.Logging;
using Refit;

namespace Infrastructure.CoinbaseExchangeProvider
{
    public class CoinbaseExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CoinbaseExchangeRateProvider> _logger;
        private readonly IMapper _mapper;
        private readonly ICoinbaseEchangeRateProvider _coinbaseRefitClient;
        private readonly Random _randomGenerator;

        public CoinbaseExchangeRateProvider(ILogger<CoinbaseExchangeRateProvider> logger,IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _randomGenerator = new Random();
            _coinbaseRefitClient = RestService.For<ICoinbaseEchangeRateProvider>("https://api.coinbase.com");
        }

        public async Task<ExchangeRate> GetCurrentExchangeRate(string baseCurrency, string targetCurrency)
        {
            var result = await _coinbaseRefitClient.GetCurrentExchangeRate(baseCurrency, targetCurrency);
            ExchangeRate exchange = _mapper.Map<ExchangeRate>(result.Data);

            //Since bitcoin price does not vary fast enough for the application a random noise is added 10%
            exchange.Price += (_randomGenerator.NextDouble() - 0.5) * exchange.Price/10d;
            exchange.Time = DateTime.Now.ToUniversalTime();
            return exchange;
        }
    }
}
using AutoMapper;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models;
using Infrastructure.CoinbaseExchangeProvider.Interface;
using Microsoft.Extensions.Logging;

namespace Infrastructure.CoinbaseExchangeProvider
{
    public class CoinbaseEchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CoinbaseEchangeRateProvider> _logger;
        private readonly IMapper _mapper;
        private readonly ICoinbaseEchangeRateProvider _coinbaseRefitClient;
        private readonly Random _randomGenerator;

        public CoinbaseEchangeRateProvider(ILogger<CoinbaseEchangeRateProvider> logger,IMapper mapper, ICoinbaseEchangeRateProvider coinbaseRefitClient)
        {
            _logger = logger;
            _mapper = mapper;
            _coinbaseRefitClient = coinbaseRefitClient;
            _randomGenerator = new Random();
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
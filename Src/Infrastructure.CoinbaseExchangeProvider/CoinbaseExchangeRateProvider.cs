using AutoMapper;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models;
using BitcoinMonitor.Domain.Models.Configuration;
using Infrastructure.CoinbaseExchangeProvider.Interface;
using Microsoft.Extensions.Logging;
using Refit;

namespace Infrastructure.CoinbaseExchangeProvider
{
    public class CoinbaseExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CoinbaseExchangeRateProvider> _logger;
        private readonly ExchangeRateProviderConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ICoinbaseEchangeRateProvider _coinbaseRefitClient;
        private readonly Random _randomGenerator;

        public CoinbaseExchangeRateProvider(ILogger<CoinbaseExchangeRateProvider> logger, ExchangeRateProviderConfiguration configuration, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
            _randomGenerator = new Random();
            _coinbaseRefitClient = RestService.For<ICoinbaseEchangeRateProvider>("https://api.coinbase.com");
        }

        public async Task<ExchangeRate> GetCurrentExchangeRate(string baseCurrency, string targetCurrency)
        {
            var result = await _coinbaseRefitClient.GetCurrentExchangeRate(baseCurrency, targetCurrency);
            ExchangeRate exchange = _mapper.Map<ExchangeRate>(result.Data);

            //Since bitcoin price does not vary fast enough for the application a random noise is added.
            //Noise level is provided through configuration
            exchange.Price += (_randomGenerator.NextDouble() - 0.5) * exchange.Price * (_configuration.NoiseLevel / 100d);
            exchange.Time = DateTime.Now.ToUniversalTime();
            return exchange;
        }
    }
}
using BitcoinMonitor.Data;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models;
using BitcoinMonitor.Domain.Models.Configuration;
using System.Reactive.Subjects;

namespace BitcoinMonitor.BackgroundServices
{
    public class ExchangeRateMonitor : BackgroundService
    {
        private readonly ILogger<ExchangeRateMonitor> _logger;
        private readonly IServiceProvider _serviceProvided;
        private readonly ExchangeRateProviderConfiguration _configuration;
        private readonly PeriodicTimer _periodicTimer;
        private BehaviorSubject<ExchangeRate?> _currentExchangeRate;

        public IObservable<ExchangeRate?> CurrentExchangeRate => _currentExchangeRate;

        public ExchangeRateMonitor(ILogger<ExchangeRateMonitor> logger, IServiceProvider serviceProvided, ExchangeRateProviderConfiguration configuration)
        {
            _logger = logger;
            _serviceProvided = serviceProvided;
            _configuration = configuration;
            _periodicTimer = new PeriodicTimer(_configuration.SamplingInterval);
            _currentExchangeRate = new BehaviorSubject<ExchangeRate?>(default);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _currentExchangeRate.OnNext(await GetExchangeRate());
                await _periodicTimer.WaitForNextTickAsync(stoppingToken);
            }
        }

        async Task<ExchangeRate> GetExchangeRate()
        {
            using var scope = _serviceProvided.CreateScope();
            var exchangeProvider = scope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
            ExchangeRate? result = await exchangeProvider.GetCurrentExchangeRate("BTC", "EUR");
            _logger.LogInformation("Retrivers echange rate for {baseCurrency} to {targetCurrency}: {price}", result.BaseCurrency, result.TargetCurrency, result.Price);
            
            var dbContext = scope.ServiceProvider.GetRequiredService<BitcoinMonitorContext>();
            var r = await dbContext.ExchangeRates.AddAsync(result);
            await dbContext.SaveChangesAsync();

            return result;
        }
    }
}

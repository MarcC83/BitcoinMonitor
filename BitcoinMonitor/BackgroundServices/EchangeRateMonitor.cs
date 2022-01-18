using BitcoinMonitor.Data;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models;

namespace BitcoinMonitor.BackgroundServices
{
    public class EchangeRateMonitor : BackgroundService
    {
        private readonly ILogger<EchangeRateMonitor> _logger;
        private readonly IServiceProvider _serviceProvided;
        private readonly PeriodicTimer _periodicTimer;
        public EchangeRateMonitor(ILogger<EchangeRateMonitor> logger, IServiceProvider serviceProvided)
        {
            _logger = logger;
            _serviceProvided = serviceProvided;
            _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
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
                await GetExchangeRate();
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

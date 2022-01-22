using BitcoinMonitor.BackgroundServices;
using BitcoinMonitor.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BitcoinMonitor.Hubs
{
    public class ExchangeRateUpdateHub : Hub<IExchangeRateHub>
    {
        private readonly ILogger<ExchangeRateUpdateHub> _logger;
        private readonly ExchangeRateMonitor _exchangeRateMonitor;
        private IDisposable? _observer;
        public ExchangeRateUpdateHub(ILogger<ExchangeRateUpdateHub> logger, ExchangeRateMonitor exchangeRateMonitor)
        {
            _logger = logger;
            _exchangeRateMonitor = exchangeRateMonitor;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected");
            _observer = _exchangeRateMonitor.CurrentExchangeRate
                .Subscribe((newValue) =>
                {
                    if (newValue is not null)
                        Clients.Caller.CurrentExchangeRate(newValue);
                });

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected");

            //Disposing observer to stop tracking of the Exchange rate state for that client.
            _observer?.Dispose();

            return base.OnDisconnectedAsync(exception);
        }
    }
}

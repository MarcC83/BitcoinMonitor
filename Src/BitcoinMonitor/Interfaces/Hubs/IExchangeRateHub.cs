using BitcoinMonitor.Domain.Models;

namespace BitcoinMonitor.Interfaces.Hubs
{
    public interface IExchangeRateHub
    {
        /// <summary>
     /// Send updates of the current exchange rate
     /// </summary>
     /// <param name="agentId">agent updated</param>
     /// <param name="agentState">updated state</param>
     /// <returns></returns>
        Task CurrentExchangeRate(ExchangeRate exchangeRate);
    }
}

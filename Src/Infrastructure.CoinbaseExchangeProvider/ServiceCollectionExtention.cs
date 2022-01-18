using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using Infrastructure.CoinbaseExchangeProvider.Interface;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infrastructure.CoinbaseExchangeProvider
{
    public static class ServiceCollectionExtention
    {
        static public void CoinbaseExchangeProvider(this IServiceCollection services, string baseUrl)
        {
            services.AddRefitClient<ICoinbaseEchangeRateProvider>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

            services.AddScoped<IExchangeRateProvider, CoinbaseEchangeRateProvider>();
        }
    }
}

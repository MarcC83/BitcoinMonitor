using AutoBogus;
using AutoMapper;
using BitcoinMonitor.Domain.Models;
using Infrastructure.CoinbaseExchangeProvider.AutoMapperProfile;
using Infrastructure.CoinbaseExchangeProvider.Models;
using Xunit;
using FluentAssertions;
using System;
using System.Globalization;

namespace Test.BitcoinMonitor
{
    public class CoinbaseExchangeRateProviderTypeMapping
    {
        IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CoinbaseAutoMapperProfile());
            });
            return configuration.CreateMapper();
        }

        [Fact]
        public void CoinbaseExchangeRateToBitcoinMonitorMapping()
        {
            //Arrange
            var exchangeRateFaker = new AutoFaker<CoinbaseExchangeRate>()
                                          .RuleFor(fake => fake.Price, fake => fake.Random.Double().ToString(CultureInfo.InvariantCulture));
                                          
            var coinbaseExchangeRateResponse = exchangeRateFaker.Generate();

            //Act
            var bitcoinMonitorExchangeRate = GetMapper().Map<ExchangeRate>(coinbaseExchangeRateResponse);

            //Assert
            bitcoinMonitorExchangeRate.BaseCurrency.Should().Be(coinbaseExchangeRateResponse.BaseCurrency);
            bitcoinMonitorExchangeRate.TargetCurrency.Should().Be(coinbaseExchangeRateResponse.TargetCurrency); 
            bitcoinMonitorExchangeRate.Price.Should().Be(Double.Parse(coinbaseExchangeRateResponse.Price, CultureInfo.InvariantCulture));
        }
    }
}
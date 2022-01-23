using BitcoinMonitor.Data;
using BitcoinMonitor.Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Test.BitcoinMonitor
{
    
    public class ExchangeRateServiceTest
    {

        BitcoinMonitorContext GetDbContext(string databaseName)
        {
            var builder = new DbContextOptionsBuilder<BitcoinMonitorContext>();
            builder.UseInMemoryDatabase(databaseName);
            return new BitcoinMonitorContext(builder.Options);
        }

        void SeedDatabase(BitcoinMonitorContext context, DateTime baseTime)
        {
            //Adding exchangeRate values with 10 seconds internal in the past
            for(int i = 0; i < 100; i++)
            {
                context.ExchangeRates.Add(
                 new ExchangeRate() { BaseCurrency ="BTC", TargetCurrency ="EUR", Price = 10, Time = baseTime.ToUniversalTime().AddSeconds(-i *10)}
                );
            }
            context.SaveChanges();
        }

        [Fact]
        async public void GetExchangeRatesTest()
        {
            //Arrange
            //Each test has its how database to allow parallel testing
            var context = GetDbContext(nameof(GetExchangeRatesTest));
            var baseTime = DateTime.Now;
            SeedDatabase(context, baseTime);

            //SUT
            var exchangeRateService = new ExchangeRateService(context);

            //Act
            var exchangeRates = await exchangeRateService.GetExchangeRatesAsync();

            //Assert
            exchangeRates.Length.Should().Be(100);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(10000)]
        async public void GetExchangeRatesTestWithTime(double seconds)
        {
            //Arrange
            //Each test has its how database to allow parallel testing
            var context = GetDbContext($"{nameof(GetExchangeRatesTestWithTime)}{seconds}");
            var baseTime = DateTime.Now;
            SeedDatabase(context, baseTime);

            //SUT
            var exchangeRateService = new ExchangeRateService(context);

            //Act
            var exchangeRates = await exchangeRateService.GetExchangeRatesAsync(baseTime.AddSeconds(-seconds));

            //Since there is data for every 10 seconds it can be calculated but also bounded to the maximum data seeded in the database
            int retreivedValueCount = Math.Min((int)(seconds / 10),100);
            //Assert
            exchangeRates.Length.Should().Be(retreivedValueCount);
        }
    }
}

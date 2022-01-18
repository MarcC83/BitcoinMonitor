using AutoMapper;
using BitcoinMonitor.Domain.Models;
using Infrastructure.CoinbaseExchangeProvider.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CoinbaseExchangeProvider.AutoMapperProfile
{
    public class CoinbaseAutoMapperProfile : Profile
    {
        public CoinbaseAutoMapperProfile()
        {
            CreateMap<CoinbaseExchangeRate, ExchangeRate>()
                .ForMember(exchangeRate => exchangeRate.Price, s => s.MapFrom(cb => Double.Parse(cb.Price, CultureInfo.InvariantCulture)));
        }
    }
}

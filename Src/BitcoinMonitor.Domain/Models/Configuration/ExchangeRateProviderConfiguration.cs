using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinMonitor.Domain.Models.Configuration
{
    public class ExchangeRateProviderConfiguration
    {
        //Interval between Samples
        public TimeSpan SamplingInterval { get; set; }

        //Noise to apply to the reteived value in %
        public double NoiseLevel { get; set; }
    }
}

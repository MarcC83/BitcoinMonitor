using System.Text.Json.Serialization;

namespace BitcoinMonitor.Domain.Models
{
    public record ExchangeRate
    {
        [JsonIgnore]
        public int Id { get; set; }

        public DateTime Time { get; set; }
        public string? BaseCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public double Price { get; set; }
    }
}

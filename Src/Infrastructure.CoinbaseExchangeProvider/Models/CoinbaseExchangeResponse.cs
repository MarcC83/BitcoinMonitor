using System.Text.Json.Serialization;

namespace Infrastructure.CoinbaseExchangeProvider.Models
{
    
    public record CoinbaseExchangeResponse
    {
        [JsonPropertyName("data")]
        public CoinbaseExchangeRate Data { get; set; }
    }

    public record CoinbaseExchangeRate
    {
        [JsonPropertyName("base")]
        public string? BaseCurrency { get; set; }

        [JsonPropertyName("currency")]
        public string? TargetCurrency { get; set; }

        [JsonPropertyName("amount")]
        public string Price { get; set; }
    }
}

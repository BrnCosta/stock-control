using System.Text.Json.Serialization;

namespace StockControl.Core.Responses.BrApi
{
    public class BrApiResponse
    {
        [JsonPropertyName("results")]
        public List<BrApiMarketResultInformation> Results { get; set; }

        [JsonPropertyName("stocks")]
        public List<BrApiMarketStockInformation> Stocks { get; set; }
    }

    public class BrApiMarketResultInformation
    {
        public required decimal RegularMarketPrice { get; set; }
    }

    public class BrApiMarketStockInformation
    {
        public required string Type { get; set; }
    }
}

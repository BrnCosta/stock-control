namespace StockControl.Core.Responses.BrApi
{
    public class BrApiResponse
    {
        public required List<BrApiMarketResultInformation> Results { get; set; }
        public required List<BrApiMarketStockInformation> Stocks { get; set; }
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

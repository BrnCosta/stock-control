using StockControl.Core.Enums;

namespace StockControl.Core.Responses
{
    public class AssetInformationResponse
    {
        public decimal RegularMarketPrice { get; set; }
        public AssetType Type { get; set; }
    }
}

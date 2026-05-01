using StockControl.Core.Enums;
using System.Text.Json.Serialization;

namespace StockControl.Core.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public OperationType OperatingType { get; set; }

        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }

        public Guid TradeId { get; set; }
        public Trade Trade { get; set; }
    }
}

using StockControl.Core.Enums;

namespace StockControl.Core.Entities
{
    public class Asset
    {
        public Guid Id { get; set; }
        public required string Ticker { get; set; }
        public decimal Price { get; set; }
        public AssetType Type { get; set; }
        public DateTime LastUpdate { get; set; }
        public Currency Currency { get; set; }

        public ICollection<Transaction> Transactions { get; } = [];
        public ICollection<Dividend> Dividends { get; } = [];
        public Position Position { get; set; }
    }
}

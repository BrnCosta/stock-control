using StockControl.Core.Enums;

namespace StockControl.Core.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public decimal? Tax { get; set; }
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }

        // Relationships
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}

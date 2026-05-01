namespace StockControl.Core.Requests
{
    public class TradeRequest
    {
        public required DateTime Date { get; set; }
        public required decimal Tax { get; set; }
        public required ICollection<TransactionRequest> Transactions { get; set; }
    }
}

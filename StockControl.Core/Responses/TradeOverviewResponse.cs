using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses
{
    public class TradeOverviewResponse
    {
        public Guid Id { get; set; }
        public decimal Tax { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public IEnumerable<TransactionOverviewResponse> Transactions { get; set; }
    }

    public class TransactionOverviewResponse
    {
        public string AssetTicker { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string OperationType { get; set; }
    }
}

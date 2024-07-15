using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Requests
{
    public class BuyStockRequest
    {
        public required string StockSymbol { get; set; }
        public required double Price { get; set; }
        public required int Quantity { get; set; }
    }
}

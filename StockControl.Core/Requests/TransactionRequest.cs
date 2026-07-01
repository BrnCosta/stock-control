using StockControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Requests
{
    public class TransactionRequest
    {
        public required string Ticker { get; set; }
        public required decimal Quantity { get; set; }
        public required decimal Price { get; set; }
        public required string OperatingType { get; set; }
    }
}

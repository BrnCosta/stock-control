using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses
{
    public class DividendGroupByMonthResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class DividendGroupByTickerResponse : DividendGroupByMonthResponse
    {
        public required string Asset { get; set; }
    }
}

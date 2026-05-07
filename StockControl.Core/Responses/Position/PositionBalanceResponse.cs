using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses.Position
{
    public class PositionBalanceResponse
    {
        public decimal TotalInvested { get; set; }
        public decimal CurrentInvested { get; set; }
        public decimal TotalGain { get; set; }
        public decimal GainPercentage { get; set; }
    }
}

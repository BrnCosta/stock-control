using StockControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses
{
  public class StockHolderOverviewResponse
  {
    public required string StockSymbol { get; set; }
    public required double AveragePrice { get; set; }
    public required int Quantity { get; set; }
    public double TotalInvested { get; set; }
    public required double Price { get; set; }
    public double CurrentPrice { get; set; }
    public double CurrentGain { get; set; }
    public double GainPercentage { get; set; }
    public required string StockType { get; set; }
  }
}

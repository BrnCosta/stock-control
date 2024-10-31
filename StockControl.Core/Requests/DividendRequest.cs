using StockControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Requests
{
  public class DividendRequest
  {
    public required string StockSymbol { get; set; }
    public required double Value { get; set; }
    public required DateOnly Date { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services.External
{
  public class StockInformationResult
  {
    public double RegularMarketPrice { get; set; }
  }

  public interface IStockInformationService
  {
    Task<StockInformationResult> GetStockInformation(string stockSymbol);
  }
}

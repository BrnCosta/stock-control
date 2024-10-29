using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Services
{
  public interface IStockService
  {
    Stock CreateIfNewStock(string stockSymbol);
  }
}

using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Services
{
  public interface IStockHolderService
  {
    List<object> GetAllStockHolders();

    StockHolder BuyStock(string stockSymbol, int buyQuantity, double buyPrice);

    StockHolder SellStock(string stockSymbol, int sellQuantity, double sellPrice);
  }
}

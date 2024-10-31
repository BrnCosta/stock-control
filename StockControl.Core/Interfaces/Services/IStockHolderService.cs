using StockControl.Core.Entities;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Services
{
  public interface IStockHolderService
  {
    List<StockHolderOverviewResponse> GetStockHoldersOverview();

    StockHolder BuyStock(string stockSymbol, int buyQuantity, double buyPrice);

    StockHolder SellStock(string stockSymbol, int sellQuantity, double sellPrice);
  }
}

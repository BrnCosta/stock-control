using StockControl.Core.Entities;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Services
{
  public interface IStockHolderService
  {
    List<StockHolderOverviewResponse> GetStockHoldersOverview();
    StockHolder CreateOrUpdateStockHolder(Stock stock, int buyQuantity, double buyPrice);
    void UpdateSellStockHolder(int sellQuantity, StockHolder stockHolder);
  }
}

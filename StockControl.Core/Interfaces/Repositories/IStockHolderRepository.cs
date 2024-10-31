using StockControl.Core.Entities;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Repositories
{
  public interface IStockHolderRepository : IBaseRepository<StockHolder>
  {
    Task<StockHolder?> GetStockHolderByStockSymbol(string stockSymbol);
    IEnumerable<StockHolderOverviewResponse> GetFilteredAll();
  }
}

using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Repositories
{
  public interface IStockHolderRepository : IBaseRepository<StockHolder>
  {
    Task<StockHolder?> GetStockHolderByStockSymbol(string stockSymbol);
    IEnumerable<object> GetFilteredAll();
  }
}

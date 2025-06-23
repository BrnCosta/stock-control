using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Repositories
{
  public interface IStockRepository : IBaseRepository<Stock>
  {
    Task<Stock?> GetAsync(string symbol);
    DateTime GetLatestUpdate();
  }
}

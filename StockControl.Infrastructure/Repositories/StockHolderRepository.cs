using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class StockHolderRepository(AppDbContext context) : BaseRepository<StockHolder>(context), IStockHolderRepository
  {
    public Task<StockHolder?> GetStockHolderByStockSymbol(string stockSymbol)
    {
      return _context.StockHolders.FirstOrDefaultAsync(x => x.StockSymbol == stockSymbol);
    }

    public IEnumerable<object> GetFilteredAll()
    {
      return _context.StockHolders
        .AsNoTracking()
        .Include(e => e.Stock)
        .Select(e => new
        {
          e.Id,
          e.AveragePrice,
          e.Quantity,
          e.StockSymbol,
          StockPrice = e.Stock.Price,
          StockType = e.Stock.StockType.ToString()
        });
    }
  }
}

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

    public new IEnumerable<StockHolder> GetAll()
    {
      return _context.StockHolders
        .AsNoTracking()
        .Include(e => e.Stock);
    }
  }
}

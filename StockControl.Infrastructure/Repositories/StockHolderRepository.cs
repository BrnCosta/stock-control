using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Responses;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class StockHolderRepository(AppDbContext context) : BaseRepository<StockHolder>(context), IStockHolderRepository
  {
    public Task<StockHolder?> GetStockHolderByStockSymbol(string stockSymbol)
    {
      return _context.StockHolders.FirstOrDefaultAsync(x => x.StockSymbol == stockSymbol);
    }

    public IEnumerable<StockHolderOverviewResponse> GetFilteredAll()
    {
      return _context.StockHolders
        .AsNoTracking()
        .Include(e => e.Stock)
        .Select(e => CalculateStockOverview(e));
    }

    private static StockHolderOverviewResponse CalculateStockOverview(StockHolder stockHolder)
    {
      var holderOverview = new StockHolderOverviewResponse()
      {
        StockSymbol = stockHolder.StockSymbol,
        AveragePrice = stockHolder.AveragePrice,
        Quantity = stockHolder.Quantity,
        StockType = stockHolder.Stock.StockType.ToString(),
        Price = stockHolder.Stock.Price,
      };

      var totalInvested = stockHolder.AveragePrice * stockHolder.Quantity;
      var currentPrice = stockHolder.Stock.Price * stockHolder.Quantity;

      holderOverview.TotalInvested = totalInvested;
      holderOverview.CurrentPrice = currentPrice;
      holderOverview.CurrentGain = currentPrice - totalInvested;
      holderOverview.GainPercentage = holderOverview.CurrentGain / totalInvested;

      return holderOverview;
    }
  }
}

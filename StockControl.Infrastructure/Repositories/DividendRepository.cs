using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Responses;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class DividendRepository(AppDbContext context) : BaseRepository<Dividend>(context), IDividendRepository
  {
    public IEnumerable<DividendGroupByMonthResponse> GetDividendsGroupedByMonth()
    {
      return _context.Dividends
        .AsNoTracking()
        .GroupBy(d => new { d.Date.Year, d.Date.Month })
        .Select(g => new DividendGroupByMonthResponse
        {
          Year = g.Key.Year,
          Month = g.Key.Month,
          TotalValue = g.Sum(d => d.Value)
        })
        .OrderBy(summary => summary.Year)
        .ThenBy(summary => summary.Month);
    }

    public IEnumerable<DividendGroupByTickerResponse> GetDividendsGroupedBySymbol()
    {
      return _context.Dividends
        .AsNoTracking()
        .GroupBy(d => new { d.Asset.Ticker, d.Date.Year, d.Date.Month })
        .Select(g => new DividendGroupByTickerResponse
        {
          Year = g.Key.Year,
          Month = g.Key.Month,
          TotalValue = g.Sum(d => d.Value),
          Asset = g.Key.Ticker
        })
        .OrderBy(summary => summary.Year)
        .ThenBy(summary => summary.Month)
        .ThenBy(summary => summary.Asset);
    }
  }
}

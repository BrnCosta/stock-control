using StockControl.Core.Entities;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Repositories
{
  public interface IDividendRepository : IBaseRepository<Dividend>
  {
    IEnumerable<DividendGroupByMonthResponse> GetDividendsGroupedByMonth();
    IEnumerable<DividendGroupBySymbolResponse> GetDividendsGroupedBySymbol();
  }
}

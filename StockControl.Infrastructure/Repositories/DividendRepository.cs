using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class DividendRepository(AppDbContext context) : BaseRepository<Dividend>(context), IDividendRepository
  {
  }
}

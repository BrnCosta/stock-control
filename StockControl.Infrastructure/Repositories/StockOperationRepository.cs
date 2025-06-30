using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class StockOperationRepository(AppDbContext context) : BaseRepository<StockOperation>(context), IStockOperationRepository
  {
    public int CreateTransaction(Transaction entity)
    {
      var newEntity = _context.Add(entity);
      return newEntity.Entity.Id!.Value;
    }
  }
}

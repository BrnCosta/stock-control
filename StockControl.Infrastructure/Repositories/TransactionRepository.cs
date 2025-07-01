using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class TransactionRepository(AppDbContext context) : BaseRepository<Transaction>(context), ITransactionRepository
  {
    public async Task<Transaction?> GetTransactionById(int transactionId)
    {
      return await _context.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(transactionId));
    }

    public IEnumerable<Transaction> GetAllWithOperations()
    {
      return _context.Transactions
        .AsNoTracking()
        .Include(e => e.StockOperations);
    }

    public IEnumerable<StockOperation> GetAllOperationsFromTransaction(int transactionId)
    {
      return _context.Set<StockOperation>()
        .Where(op => op.TransactionId == transactionId)
        .AsNoTracking();
    }
  }
}

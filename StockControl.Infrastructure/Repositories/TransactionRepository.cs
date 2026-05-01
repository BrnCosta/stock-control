using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext context) : BaseRepository<Transaction>(context), ITransactionRepository
    {
    }
}

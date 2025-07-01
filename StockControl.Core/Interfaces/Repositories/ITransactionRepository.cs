using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Repositories
{
  public interface ITransactionRepository : IBaseRepository<Transaction>
  {
    Task<Transaction?> GetTransactionById(int transactionId);
    IEnumerable<Transaction> GetAllWithOperations();
    IEnumerable<StockOperation> GetAllOperationsFromTransaction(int transactionId);
  }
}

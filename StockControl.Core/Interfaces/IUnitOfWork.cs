using StockControl.Core.Interfaces.Repositories;

namespace StockControl.Core.Interfaces
{
  public interface IUnitOfWork
  {
    IStockRepository StockRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    IStockHolderRepository StockHolderRepository { get; }
    IDividendRepository DividendRepository { get; }

    Task Commit();
  }
}

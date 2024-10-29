using StockControl.Core.Interfaces.Repositories;

namespace StockControl.Core.Interfaces
{
  public interface IUnitOfWork
  {
    IStockRepository StockRepository { get; }
    IStockOperationRepository OperationRepository { get; }
    IStockHolderRepository StockHolderRepository { get; }

    Task Commit();
  }
}

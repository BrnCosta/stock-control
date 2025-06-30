using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Requests;

namespace StockControl.Core.Interfaces.Services
{
  public interface IOperationService
  {
    List<StockOperation> GetAllOperations();

    void CreateNewTransaction(TransactionRequest transaction);
  }
}

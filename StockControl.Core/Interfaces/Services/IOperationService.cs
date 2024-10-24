using StockControl.Core.Entities;
using StockControl.Core.Enums;

namespace StockControl.Core.Interfaces.Services
{
  public interface IOperationService
  {
    List<StockOperation> GetAllOperations();

    void CreateNewOperation(string stockSymbol, double price, int quantity, OperationType operation);
  }
}

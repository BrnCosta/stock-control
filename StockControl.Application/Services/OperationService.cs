using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;

namespace StockControl.Application.Services
{
  public class OperationService(IUnitOfWork unitOfWork) : IOperationService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<StockOperation> GetAllOperations()
    {
      return _unitOfWork.OperationRepository.GetAll().ToList();
    }

    public void CreateNewOperation(string stockSymbol, double price, int quantity, OperationType operation)
    {
      var stockOperation = new StockOperation
      {
        Date = DateTime.Now,
        OperatingType = operation,
        Quantity = quantity,
        Price = price,
        StockSymbol = stockSymbol,
      };

      _unitOfWork.OperationRepository.Create(stockOperation);
    }
  }
}

using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.Application.Services
{
  public class OperationService(IUnitOfWork unitOfWork, IStockService stockService, IStockHolderService holderService) 
    : IOperationService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IStockService _stockService = stockService;
    protected readonly IStockHolderService _holderService = holderService;

    public List<StockOperation> GetAllOperations()
    {
      return _unitOfWork.OperationRepository.GetAll().ToList();
    }

    public void CreateNewTransaction(TransactionRequest transactionRequest)
    {
      var transaction = new Transaction
      {
        Value = transactionRequest.Value,
        Date = DateOnly.FromDateTime(DateTime.Now),
        StockOperations = transactionRequest.StockOperations.Select(op => new StockOperation
        {
          StockSymbol = op.StockSymbol,
          Quantity = op.Quantity,
          Price = op.Price,
          OperatingType = op.OperatingType
        }).ToList()
      };

      var transactionId = _unitOfWork.OperationRepository.CreateTransaction(transaction);

      foreach(var operation in transaction.StockOperations)
        CreateNewOperation(operation, transactionId);

      _unitOfWork.Commit();
    }

    private void CreateNewOperation(StockOperation operation, int transactionId)
    {
      operation.TransactionId = transactionId;

      if (operation.OperatingType == OperationType.Buy)
        BuyStock(operation);

      if (operation.OperatingType == OperationType.Sell)
        SellStock(operation);

      _unitOfWork.OperationRepository.Create(operation);
    }

    public void BuyStock(StockOperation operation)
    {
      try
      {
        Stock stock = _stockService.CreateIfNewStock(operation.StockSymbol);

        _holderService.CreateOrUpdateStockHolder(stock, operation.Quantity, operation.Price);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error while buying stock: {ex.Message}", ex);
      }
    }

    public void SellStock(StockOperation operation)
    {
      try
      {
        StockHolder? stockHolder = _unitOfWork.StockHolderRepository.GetStockHolderByStockSymbol(operation.StockSymbol)
        .GetAwaiter().GetResult() ?? throw new ArgumentException($"There is no available stock {operation.StockSymbol} to sell.");

        if (stockHolder.Quantity < operation.Quantity)
          throw new ArgumentException($"Quantity informed is bigger than avaiable. " +
            $"Quantity: {operation.Quantity} Available: {stockHolder.Quantity}");

        _holderService.UpdateSellStockHolder(operation.Quantity, stockHolder);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error while selling stock: {ex.Message}", ex);
      }
    }
  }
}

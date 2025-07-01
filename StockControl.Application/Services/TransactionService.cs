using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.Application.Services
{
  public class TransactionService(IUnitOfWork unitOfWork, IStockService stockService, 
    IStockHolderService holderService) : ITransactionService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IStockService _stockService = stockService;
    protected readonly IStockHolderService _holderService = holderService;

    public List<Transaction> GetAllTransactions()
    {
      return _unitOfWork.TransactionRepository.GetAllWithOperations().ToList();
    }

    public List<StockOperation> GetAllOperationsFromTransaction(int transactionId)
    {
      return _unitOfWork.TransactionRepository.GetAllOperationsFromTransaction(transactionId).ToList();
    }

    public void UpdateTax(UpdateTaxRequest updateRequest)
    {
      Transaction? transaction = _unitOfWork.TransactionRepository
        .GetTransactionById(updateRequest.TransactionId)
        .GetAwaiter().GetResult() ?? throw new Exception("Transaction not found.");

      transaction.Tax = updateRequest.TaxValue;

      _unitOfWork.TransactionRepository.Update(transaction);
      _unitOfWork.Commit();
    }

    public void CreateNewTransaction(TransactionRequest transactionRequest)
    {
      try
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

        _unitOfWork.TransactionRepository.Create(transaction);

        VerifyOperations(transaction.StockOperations);

        _unitOfWork.Commit();
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed to confirm transaction: {ex.Message}", ex);
      }
    }

    private void VerifyOperations(ICollection<StockOperation> operations)
    {
      foreach (var operation in operations)
      {
        if (operation.OperatingType == OperationType.Buy)
          BuyStock(operation);

        if (operation.OperatingType == OperationType.Sell)
          SellStock(operation);
      }
    }

    private void BuyStock(StockOperation operation)
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

    private void SellStock(StockOperation operation)
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

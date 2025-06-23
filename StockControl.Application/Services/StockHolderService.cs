using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;

namespace StockControl.Application.Services
{
  public class StockHolderService(IUnitOfWork unitOfWork, IStockService stockService, IOperationService operationService) : IStockHolderService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IStockService _stockService = stockService;
    protected readonly IOperationService _operationService = operationService;

    public List<object> GetAllStockHolders()
    {
      return _unitOfWork.StockHolderRepository.GetFilteredAll().ToList();
    }

    private StockHolder CreateStockHolder(Stock stock, int buyQuantity, double buyPrice)
    {
      try
      {
        StockHolder stockHolder = new()
        {
          AveragePrice = buyPrice,
          Quantity = buyQuantity,
          Stock = stock,
          StockSymbol = stock.Symbol,
        };

        _unitOfWork.StockHolderRepository.Create(stockHolder);

        return stockHolder;
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed to create new StockHolder: {ex.Message}", ex);
      }
    }

    private StockHolder UpdateBuyStockHolder(int buyQuantity, double buyPrice, StockHolder stockHolder)
    {
      try
      {
        double averagePrice = CalculateAveragePrice(stockHolder, buyPrice, buyQuantity);
        stockHolder.AveragePrice = averagePrice;
        stockHolder.Quantity += buyQuantity;

        _unitOfWork.StockHolderRepository.Update(stockHolder);

        return stockHolder;
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed to update StockHolder: {ex.Message}", ex);
      }
    }

    private StockHolder CreateOrUpdateStockHolder(Stock stock, int buyQuantity, double buyPrice)
    {
      StockHolder? stockHolder = _unitOfWork.StockHolderRepository.GetStockHolderByStockSymbol(stock.Symbol).GetAwaiter().GetResult();

      if (stockHolder is null)
        return CreateStockHolder(stock, buyQuantity, buyPrice);

      return UpdateBuyStockHolder(buyQuantity, buyPrice, stockHolder);
    }

    public StockHolder BuyStock(string stockSymbol, int buyQuantity, double buyPrice)
    {
      try
      {
        Stock stock = _stockService.CreateIfNewStock(stockSymbol);

        _operationService.CreateNewOperation(stock.Symbol, buyPrice, buyQuantity, OperationType.Buy);

        StockHolder stockHolder = CreateOrUpdateStockHolder(stock, buyQuantity, buyPrice);

        _unitOfWork.Commit();

        return stockHolder;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error while buying stock: {ex.Message}", ex);
      }
    }

    private StockHolder UpdateSellStockHolder(int sellQuantity, StockHolder stockHolder)
    {
      try
      {
        stockHolder.Quantity -= sellQuantity;

        _unitOfWork.StockHolderRepository.Update(stockHolder);

        return stockHolder;
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed to update StockHolder: {ex.Message}", ex);
      }
    }

    public StockHolder SellStock(string stockSymbol, int sellQuantity, double sellPrice)
    {
      StockHolder? stockHolder = _unitOfWork.StockHolderRepository.GetStockHolderByStockSymbol(stockSymbol).GetAwaiter().GetResult() 
        ?? throw new ArgumentException($"There is no avaiable stock {stockSymbol} to sell.");

      if (stockHolder.Quantity < sellQuantity)
        throw new ArgumentException($"Quantity informed is bigger than avaiable. Quantity: {sellQuantity} Available: {stockHolder.Quantity}");

      stockHolder = UpdateSellStockHolder(sellQuantity, stockHolder);

      _operationService.CreateNewOperation(stockHolder.StockSymbol, sellPrice, sellQuantity, OperationType.Sell);

      _unitOfWork.Commit();

      return stockHolder;
    }

    private static double CalculateAveragePrice(StockHolder stockHolder, double buyPrice, int buyQuantity)
    {
      try
      {
        double currentPrice = stockHolder.AveragePrice * stockHolder.Quantity;
        int totalQuantity = buyQuantity + stockHolder.Quantity;

        double newAveragePrice = (currentPrice + (buyPrice * buyQuantity)) / totalQuantity;

        return newAveragePrice;
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed while calculating the new average price: {ex.Message}", ex);
      }
    }
  }
}

using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using System.Diagnostics.SymbolStore;

namespace StockControl.Application.Services
{
  public class StockService(IUnitOfWork unitOfWork)
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<StockHolder> GetAllStockHolders()
    {
      return _unitOfWork.StockHolderRepository.GetAll().ToList();
    }

    private Stock CreateIfNewStock(string stockSymbol)
    {
      Stock? stock = _unitOfWork.StockRepository.GetAsync(stockSymbol).GetAwaiter().GetResult();

      if (stock is null)
      {
        stock = GetStockInformation(stockSymbol);
        _unitOfWork.StockRepository.Create(stock);
      }

      return stock;
    }

    // TODO: Get information from internet
    private static Stock GetStockInformation(string stockSymbol)
    {
      return new Stock
      {
        Symbol = stockSymbol,
        Price = 0.0,
        StockType = StockType.Stock
      };
    }

    public void CreateNewOperation(Stock stock, double buyPrice, int quantity)
    {
      var stockOperation = new StockOperation
      {
        Date = DateTime.Now,
        OperatingType = OperationType.Buy,
        Quantity = quantity,
        Price = buyPrice,
        StockSymbol = stock.Symbol,
      };

      _unitOfWork.OperationRepository.Create(stockOperation);
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

    private StockHolder UpdateStockHolder(int buyQuantity, double buyPrice, StockHolder stockHolder)
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

      return UpdateStockHolder(buyQuantity, buyPrice, stockHolder);
    }

    public StockHolder BuyNewStock(string stockSymbol, int buyQuantity, double buyPrice)
    {
      try
      {
        Stock stock = CreateIfNewStock(stockSymbol);

        CreateNewOperation(stock, buyPrice, buyQuantity);

        StockHolder stockHolder = CreateOrUpdateStockHolder(stock, buyQuantity, buyPrice);

        _unitOfWork.Commit();

        return stockHolder;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error while buying stock: {ex.Message}", ex);
      }
    }

    private static double CalculateAveragePrice(StockHolder stockHolder, double buyPrice, int buyQuantity)
    {
      try
      {
        double actualPrice = stockHolder.AveragePrice * stockHolder.Quantity;
        int totalQuantity = buyQuantity + stockHolder.Quantity;

        double newAveragePrice = (actualPrice + (buyPrice * buyQuantity)) / totalQuantity;

        return newAveragePrice;
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed while calculating the new average price: {ex.Message}", ex);
      }
    }
  }
}

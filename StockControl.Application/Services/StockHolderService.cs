using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
  public class StockHolderService(IUnitOfWork unitOfWork) : IStockHolderService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<StockHolderOverviewResponse> GetStockHoldersOverview()
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

    public StockHolder CreateOrUpdateStockHolder(Stock stock, int buyQuantity, double buyPrice)
    {
      StockHolder? stockHolder = _unitOfWork.StockHolderRepository.GetStockHolderByStockSymbol(stock.Symbol).GetAwaiter().GetResult();

      if (stockHolder is null)
        return CreateStockHolder(stock, buyQuantity, buyPrice);

      return UpdateBuyStockHolder(buyQuantity, buyPrice, stockHolder);
    }

    public void UpdateSellStockHolder(int sellQuantity, StockHolder stockHolder)
    {
      try
      {
        stockHolder.Quantity -= sellQuantity;

        _unitOfWork.StockHolderRepository.Update(stockHolder);
      }
      catch (Exception ex)
      {
        throw new Exception($"Failed to update StockHolder: {ex.Message}", ex);
      }
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

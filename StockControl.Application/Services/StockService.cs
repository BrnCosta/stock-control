using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;

namespace StockControl.Application.Services
{
    public class StockService(IUnitOfWork unitOfWork)
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;

        public StockHolder BuyNewStock(string stockSymbol, int buyQuantity, double buyPrice)
        {
            Stock stock = GetStockInformation(stockSymbol);

            _unitOfWork.CreateIfNewStock(stock);

            _unitOfWork.CreateNewOperation(stock, buyPrice, buyQuantity);

            StockHolder? stockHolder = _unitOfWork.GetStockHolderByStock(stockSymbol).GetAwaiter().GetResult();

            if(stockHolder != null)
            {
                double averagePrice = CalculateAveragePrice(stockHolder, buyPrice, buyQuantity);
                stockHolder.AveragePrice = averagePrice;
                stockHolder.Quantity += buyQuantity;

                _unitOfWork.UpdateStockHolder(stockHolder);
            }
            else 
            {
                stockHolder = new StockHolder
                {
                    AveragePrice = buyPrice,
                    Quantity = buyQuantity,
                    Stock = stock,
                    StockSymbol = stock.Symbol,
                };

                _unitOfWork.CreateStockHolder(stockHolder);
            }

            _unitOfWork.Commit();

            return stockHolder;
        }

        private Stock GetStockInformation(string stockSymbol)
        {
            return new Stock
            {
                Symbol = stockSymbol,
                Price = 0.0,
                StockType = StockType.Stock
            };
        }

        private double CalculateAveragePrice(StockHolder stockHolder, double buyPrice, int buyQuantity)
        {
            double actualPrice = stockHolder.AveragePrice * stockHolder.Quantity;
            int totalQuantity = buyQuantity + stockHolder.Quantity;

            double newAveragePrice = (actualPrice + (buyPrice * buyQuantity)) / totalQuantity;

            return newAveragePrice;
        }
    }
}

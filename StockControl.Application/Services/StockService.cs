using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Application.Services
{
  public class StockService(IUnitOfWork unitOfWork) : IStockService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public Stock CreateIfNewStock(string stockSymbol)
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
  }
}

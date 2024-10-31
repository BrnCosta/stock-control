using StockControl.Application.Services.External;
using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Interfaces.Services.External;

namespace StockControl.Application.Services
{
  public class StockService(IUnitOfWork unitOfWork, IStockInformationService stockInformationService) : IStockService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IStockInformationService _stockInformationService = stockInformationService;

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

    private static StockType GetStockTypeBaseOnSymbol(string symbol)
    {
      if (symbol.Length < 5)
        return StockType.Crypto;

      int symbolNumber = Int16.Parse(symbol[4..]);

      return symbolNumber switch
      {
        3 or 4 => StockType.Stock,
        9 or 11 => StockType.RealEstateFund,
        34 => StockType.BDRs,
        _ => StockType.Crypto,
      };
    }

    private Stock GetStockInformation(string stockSymbol)
    {
      StockInformationResult stockInformation = _stockInformationService.GetStockInformation(stockSymbol).GetAwaiter().GetResult();

      return new Stock
      {
        Symbol = stockSymbol,
        Price = stockInformation.RegularMarketPrice,
        StockType = GetStockTypeBaseOnSymbol(stockSymbol),
        LastUpdate = DateTime.Now,
      };
    }
  }
}

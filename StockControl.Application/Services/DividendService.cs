using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
  public class DividendService(IUnitOfWork unitOfWork) : IDividendService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<Dividend> GetAll()
    {
      return _unitOfWork.DividendRepository.GetAll().ToList();
    }

    public List<DividendGroupByMonthResponse> GetGroupByMonth()
    {
      return _unitOfWork.DividendRepository.GetDividendsGroupedByMonth().ToList();
    }

    public List<DividendGroupBySymbolResponse> GetGroupBySymbol()
    {
      return _unitOfWork.DividendRepository.GetDividendsGroupedBySymbol().ToList();
    }

    public void CreateNewDividend(string stockSymbol, double value, DateOnly date)
    {
      _ = _unitOfWork.StockHolderRepository.GetStockHolderByStockSymbol(stockSymbol).GetAwaiter().GetResult()
        ?? throw new ArgumentException("It is not possible to add dividend from a not holder stock");

      var dividend = new Dividend()
      {
        StockSymbol = stockSymbol,
        Value = value,
        Date = date
      };

      _unitOfWork.DividendRepository.Create(dividend);

      _unitOfWork.Commit();
    }
  }
}
